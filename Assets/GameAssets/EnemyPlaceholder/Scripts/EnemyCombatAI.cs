using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

//Reference the type of enemy
public enum EnemyTypes
{
    Skeleton,
    SkeletonArcher,
    Werewolf,
    Spirit
};

public class EnemyCombatAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] public Transform target;
    [SerializeField] private Health health;
    [SerializeField] private EnemyMovementAI enemyMovementAi;
    [SerializeField] private GameObject projectileObj;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private VisualEffect headShockwave;
    [SerializeField] private SkinnedMeshRenderer wolfSkinMesh;

    [SerializeField] private int basicAtkDamage = 10;
    [SerializeField] private float basicAtkCooldown = 10, attackDistance = 10, projectileSpeed = 10, projectileGrowTime = 5, roarAttackStunDuration = 7f, roarDistance = 3f;

    [Header("Debug")]
    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool canSpecialAttack = false;
    [SerializeField] private int randomNum;

    [SerializeField] private float blockDuration, blockCooldown, timer = 0, normalizedTimer = 0;
    private GameObject projectileSpawned;


    public bool canBlock = true, isBlocking = false, isBasicAttacking = false, playerTakenDamage = false, isSpecialAttacking = false, nullDamageFromShield, isChargingUp = false, isSecondPhase = false;
    public List<GameObject> hitEnemyColliders = new List<GameObject>();
    public string colliderNameReference;

    [SerializeField] private EnemyTypes EnemyType;

    private void Start()
    {
        if (animator != null)
        {
            animator = GetComponent<Animator>();
        }
        
        if (health != null)
        {
            health = GetComponent<Health>();
        }
    }
    private void Update()
    {
        // Change moveset according to type of enemy it is
        switch (EnemyType)
        {
            case EnemyTypes.Skeleton:
                EnemySkeletonMoveset();
                break;
            case EnemyTypes.Werewolf:
                EnemyWerewolfMoveset();
                break;
            case EnemyTypes.Spirit:
                EnemySpiritMoveset();
                break;
        }
        
        // Calculate if hit player
        if (!playerTakenDamage && hitEnemyColliders.Count != 0)
        {
            
            foreach (GameObject obj in hitEnemyColliders)
            {
                if (obj.GetComponent<Health>() != null)
                {
                    Health health = obj.GetComponent<Health>();

                    if (!obj.GetComponent<PlayerMovement>().isDodging)
                    {
                        health.TakeDamage(basicAtkDamage);
                    }
                    
                }
            }

            playerTakenDamage = true;
        }
    }

    private void EnemySkeletonMoveset()
    {
        // Checks if player is in front and in range
        if (Vector3.Distance(transform.position, target.position) <= attackDistance && canAttack && !health.isDead)
        {
            if (enemyMovementAi.IsInFieldOfView())
            {
                if (Random.Range(0f, 1f) > 0.5f)
                {
                    if (canAttack)
                    {
                        canAttack = false;
                        animator.SetTrigger("DoAttack");

                        colliderNameReference = "sword";

                        Invoke(nameof(ResetAttack), basicAtkCooldown);
                    }
                }
                else
                {
                    if (canBlock)
                    {
                        Block();
                        Invoke(nameof(BlockLengthDuration), blockDuration);
                    }
                }

            }

        }

        // Blocks players attack
        animator.SetBool("IsBlocking", isBlocking);

        // Set the mechanics of block
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Block"))
        {
            nullDamageFromShield = true;
        }
        else
        {
            nullDamageFromShield = false;
        }
    }

    private void EnemyWerewolfMoveset()
    {
        if (Vector3.Distance(transform.position, target.position) <= attackDistance && canAttack && !health.isDead)
        {
            // Between numbers 1 to 10, 1/5 | 20% chance to occur
            if (randomNum >= 8)
            {
                canSpecialAttack = true;
                randomNum = 0;
            }

            if (enemyMovementAi.IsInFieldOfView() && canAttack && !canSpecialAttack)
            {
                float randomNumber = Random.Range(0f, 1f);
                canAttack = false;

                animator.SetFloat("DoAttackSwipeRandom", randomNumber);
                animator.SetTrigger("DoAttack");

                if (randomNumber > .5f)
                {
                    colliderNameReference = "mixamorig:RightHand";
                }
                else
                {
                    colliderNameReference = "mixamorig:LeftHand";
                }
                

                randomNum = Random.Range(1, 10);
                Invoke(nameof(ResetAttack), basicAtkCooldown);
            }
            else if (!enemyMovementAi.IsInFieldOfView() && canAttack && !canSpecialAttack)
            {
                canAttack = false;
                animator.SetTrigger("DoLegSweepAttack");

                colliderNameReference = "mixamorig:RightFoot";

                randomNum = Random.Range(1, 10);
                Invoke(nameof(ResetAttack), basicAtkCooldown);
            }
            else if (canSpecialAttack && canAttack)
            {
                canAttack = false;
                canSpecialAttack = false;

                if (!isSecondPhase)
                {
                    animator.SetTrigger("DoRoarAttack");
                }
                else
                {
                    if (Random.Range(0f, 1f) < 0.5)
                    {
                        animator.SetTrigger("DoRoarAttack");
                    }
                    else
                    {
                        animator.SetTrigger("DoJumpAttack");
                    }
                }
                

                Invoke(nameof(ResetAttack), basicAtkCooldown);
            }
        }

        if (Vector3.Distance(transform.position, target.position) <= roarDistance && isSpecialAttacking && !target.GetComponent<Animator>().GetBool("IsStunned"))
        {
            target.GetComponent<Animator>().SetBool("IsStunned", true);
            Invoke(nameof(stopStunPlayer), roarAttackStunDuration);
        }

        if (health.health <= health.maxHealth / 2 && !isSecondPhase)
        {
            isSecondPhase = true;
            animator.SetTrigger("DoSecondPhase");
        }
    }

    private void EnemySpiritMoveset()
    {
        shootingPoint.LookAt(target.position);
                
        if (Vector3.Distance(transform.position, target.position) <= attackDistance && canAttack && !health.isDead)
        {
            if (canAttack)
            {
                canAttack = false;
                isChargingUp = true;
                timer = 0;
                normalizedTimer = 0;

                GameObject enemyProjectile = Instantiate(projectileObj, shootingPoint.position, shootingPoint.rotation);
                enemyProjectile.GetComponent<ProjectileBehaviour>().speed = 0;
                enemyProjectile.GetComponent<ProjectileBehaviour>().damage = basicAtkDamage;
                projectileSpawned = enemyProjectile;
            }    
        }

        if (isChargingUp)
        {
            if (timer < projectileGrowTime)
            {
                timer += Time.deltaTime;
                normalizedTimer = timer / projectileGrowTime;

                projectileSpawned.transform.localScale = Vector3.one * normalizedTimer;
                projectileSpawned.transform.position = shootingPoint.position;
                projectileSpawned.transform.rotation = shootingPoint.rotation;
            }
            else
            {
                projectileSpawned.GetComponent<ProjectileBehaviour>().speed = projectileSpeed;
                Invoke(nameof(ResetAttack), basicAtkCooldown);

                isChargingUp = false;
            }
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    public void EnemyAttackStart()
    {
        isBasicAttacking = true;
    }

    public void EnemyAttackEnd()
    {
        isBasicAttacking = false;
        
        hitEnemyColliders.Clear();
        colliderNameReference = null;
        playerTakenDamage = false;
    }

    private void Block()
    {
        if (canBlock)
        {
            canBlock = false;
            isBlocking = true;

            Invoke(nameof(ResetBlock), blockCooldown);
        }
        
    }
        
    private void BlockLengthDuration()
    {
        isBlocking = false;
    }

    private void ResetBlock()
    {
        canBlock = true;
    }

    public void TriggerDamageAnim()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Stagger"))
        {
            animator.Play("Base Layer.Sword And Shield Impact", 0, 0.1f);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Block"))
        {
            animator.Play("Base Layer.Sword And Shield Impact With Shield", 0, 0.1f);
        }
        
    }

    public void EnemyRoaringStart()
    {
        isSpecialAttacking = true;
        InvokeRepeating(nameof(ShockwaveRepeating), 0, 0.3f);
    }

    public void EnemyRoaringEnd()
    {
        isSpecialAttacking = false;
        CancelInvoke(nameof(ShockwaveRepeating));
    }

    private void ShockwaveRepeating()
    {
        headShockwave.Play();
    }

    private IEnumerator ShockwaveRepeatOnTimer(float duration, float refreshRate)
    {
        float timer = 0;

        while (timer < duration)
        {
            yield return new WaitForSeconds(refreshRate);

            headShockwave.Play();
            timer += refreshRate;
        }
    }

    private void stopStunPlayer()
    {
        target.GetComponent<Animator>().SetBool("IsStunned", false);
    }

    public void EnterSecondPhase()
    {
        wolfSkinMesh.materials[2].SetColor("_Emissive_Color", Color.white * 2);

        StartCoroutine(ShockwaveRepeatOnTimer(1.5f, 0.3f));
    }

}
    