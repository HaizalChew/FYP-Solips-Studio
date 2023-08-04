using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.AI;

public class AldricAI : MonoBehaviour
{
    [Header("Spells")]
    [SerializeField] private GameObject spellLightning;
    [SerializeField] private VisualEffect spellAbsorbVFX;
    [SerializeField] private GameObject teleportEffect;
    [SerializeField] private GameObject GroundWave;

    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private Animator animator;
    [SerializeField] private Health health;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private MeshRenderer SwordMeshRenderer;
    [SerializeField] private float attackRate = 3f, attackDistance = 2f, fieldOfViewAngle = 60f, rotateSpeed = 10f, teleportDistFromTarget = 5f, teleportAngle;
    [SerializeField] private float spellLightningAttackRate = 0.3f, spellLightningAttackDuration = 5f;
    [SerializeField] private int bossDamage = 30;


    [Header("Debug")]
    [SerializeField] private bool canAttack;
    [SerializeField] private bool isAttackCooldownOver, canMagicAttack, canMove, canTeleport, swordJuicedUp, isplayAbsorbGlow, isMeeleRange, playerTakenDamage;

    public bool isCurrentlyAttacking;
    public List<GameObject> hitColliders = new List<GameObject>();

    Vector3 dir;
    float fresnelVar = 2.5f;
    AttackType attackType;

    private enum AttackType
    {
        BasicAttack,
        ChargeUp,
        JumpAttack,
        MagicAttack
    }

    private void Start()
    {
        SwordMeshRenderer.materials[1].SetFloat("_FresnelPower", 2.5f);
        swordJuicedUp = false;
    }

    private void Update()
    {
        dir = target.position - transform.position;

        Movements();
        Attacks();

        // Calculate if hit player
        if (!playerTakenDamage && hitColliders.Count != 0)
        {

            foreach (GameObject obj in hitColliders)
            {
                if (obj.GetComponent<Health>() != null)
                {
                    Health health = obj.GetComponent<Health>();

                    health.TakeDamage(bossDamage);
                }
            }

            playerTakenDamage = true;
        }

        // Miscealleanous stuff
        if (isplayAbsorbGlow)
        {   
            float decreaseFresnelSpeed = 2f;

            if (SwordMeshRenderer.materials[1].GetFloat("_FresnelPower") > 0)
            {
                fresnelVar -= decreaseFresnelSpeed * Time.deltaTime;
                SwordMeshRenderer.materials[1].SetFloat("_FresnelPower", fresnelVar);
            }
            else
            {
                isplayAbsorbGlow = false;
            }
        }
    }

    private void Movements()
    {
        // Handle Movements
        if (Vector3.Distance(transform.position, target.position) > attackDistance && canMove && !health.isDead)
        {
            agent.SetDestination(target.position);
            agent.isStopped = false;
        }
        // Rotate when reached destination
        else if (Vector3.Distance(transform.position, target.position) <= attackDistance && canMove)
        {
            float singleStep = rotateSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, dir, singleStep, 0.0f);

            transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(newDirection).eulerAngles.y, 0f);
        }
        else
        {
            agent.isStopped = true;
        }


        // Active when moving
        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        canMove = !animator.GetCurrentAnimatorStateInfo(0).IsTag("Unmove");

        // Handle Teleports
        if (canTeleport)
        {
            canTeleport = false;
            TeleportAroundThePlayer();
        }

        
    }

    private void TeleportAroundThePlayer()
    {
        // Teleport around the player
        Vector3 pos = new Vector3();

        //Randomise teleport location
        teleportAngle = Random.Range(0, 360);

        // Check if teleportation is valid


        // Set teleport position
        pos.x = target.position.x + (teleportDistFromTarget * Mathf.Cos(teleportAngle / (180f / Mathf.PI)));
        pos.y = target.position.y;
        pos.z = target.position.z + (teleportDistFromTarget * Mathf.Sin(teleportAngle / (180f / Mathf.PI)));

        Vector3 relativePos = target.position - pos;

        // Teleport Player to location
        TeleportToLocation(pos, Quaternion.LookRotation(relativePos));
    }

    private void Attacks()
    {
        // Brains of the operation
        if (!canAttack && isAttackCooldownOver)
        {
            // Decide how we wanna attack
            if (!swordJuicedUp && !canMagicAttack && IsInMeeleRange())
            {
                attackType = AttackType.BasicAttack;
            }
            else if (!swordJuicedUp && !canMagicAttack)
            {
                attackType = AttackType.ChargeUp;
            }
            else if (swordJuicedUp)
            {
                attackType = AttackType.JumpAttack;
            }
            else if (canMagicAttack)
            {
                attackType = AttackType.MagicAttack;
            }
            canAttack = true;
            Debug.Log(attackType);
        }

        // Handle the animations
        if (canAttack)
        {
            canAttack = false;
            isAttackCooldownOver = false;

            // Types of animations
            switch (attackType)
            {
                case AttackType.BasicAttack:

                    animator.SetTrigger("DoAttack" + Random.Range(1, 4).ToString());
                    break;

                case AttackType.ChargeUp:

                    animator.SetTrigger("DoChargeUp");
                    
                    break;

                case AttackType.JumpAttack:

                    animator.SetTrigger("DoJumpAttack");
                    break;

                case AttackType.MagicAttack:

                    canMagicAttack = false;

                    // Trigger Spell 1: Follow Lightning Attack
                    animator.SetTrigger("DoLightningAttack");
                    Invoke(nameof(ResetMagicAttack), attackRate * 3);
                    break;
            }
            

            Invoke(nameof(ResetAttack), attackRate);
        }
    }

    private void FollowLightningAttack()
    {
        GameObject lightning = Instantiate(spellLightning, target.position, Quaternion.identity);
    }

    public void StartLightningAttack()
    {
        InvokeRepeating(nameof(FollowLightningAttack), 0, spellLightningAttackRate);
        StartCoroutine(ResetInvoke("FollowLightningAttack", spellLightningAttackDuration));
    }

    private void ResetAttack()
    {
        isAttackCooldownOver = true;
    }

    private IEnumerator ResetInvoke(string methodName, float duration)
    {
        yield return new WaitForSeconds(duration);

        CancelInvoke(methodName);
    }

    private void TeleportToLocation(Vector3 teleportPos, Quaternion teleportRot)
    {
        Instantiate(teleportEffect, transform.position, Quaternion.identity);
        transform.position = teleportPos;
        transform.rotation = teleportRot;
    }

    public void StartPlayingAbsorb()
    {
        spellAbsorbVFX.Play();
        isplayAbsorbGlow = true;
        swordJuicedUp = true;
    }

    public void ResetMagicAttack()
    {
        canMagicAttack = true;
    }   

    private bool IsInMeeleRange()
    {
        if (Mathf.Abs(Vector3.Angle(transform.forward, dir)) < fieldOfViewAngle && Vector3.Distance(transform.position, target.position) <= attackDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StandingJumpAttackWave()
    {
        GameObject groundWaveAttack = Instantiate(GroundWave, transform.position, transform.rotation);

        GroundSlash groundSlashScript = groundWaveAttack.GetComponent<GroundSlash>();

        groundWaveAttack.GetComponent<Rigidbody>().velocity = transform.forward * groundSlashScript.speed;

        SwordMeshRenderer.materials[1].SetFloat("_FresnelPower", 2.5f);
        swordJuicedUp = false;
    }

    public void TeleportAtStartOfJump()
    {
        canTeleport = true;
    }

    public void AldricBasicAttackStart()
    {
        isCurrentlyAttacking = true;
    }

    public void AldricBasicAttackEnd()
    {
        isCurrentlyAttacking = false;

        hitColliders.Clear();
        playerTakenDamage = false;
    }
}
