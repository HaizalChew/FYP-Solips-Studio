using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Reference the type of enemy
public enum EnemyTypes
{
    Skeleton,
    Werewolf
};  

public class EnemyCombatAI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    [SerializeField] private Health health;
    [SerializeField] private EnemyMovementAI enemyMovementAi;

    [SerializeField] private int basicAtkDamage;
    [SerializeField] private float basicAtkCooldown, attackDistance;

    [SerializeField] private bool canAttack = true, canSpecialAttack = false;
    [SerializeField] private int randomNum;

    // Blocking variables for skeleton
    [SerializeField] private float blockDuration, blockCooldown;

    public bool canBlock = true, isBlocking = false, isBasicAttacking = false, nullDamageFromShield;
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
        }
        
        // Calculate if hit player
        if (!isBasicAttacking && hitEnemyColliders.Count != 0)
        {
            foreach (GameObject obj in hitEnemyColliders)
            {
                if (obj.GetComponent<Health>() != null)
                {
                    Health health = obj.GetComponent<Health>();

                    health.TakeDamage(basicAtkDamage);
                }
            }

            hitEnemyColliders.Clear();
            colliderNameReference = null;
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
                animator.SetTrigger("DoRoarAttack");

                Invoke(nameof(ResetAttack), basicAtkCooldown);
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
}
    