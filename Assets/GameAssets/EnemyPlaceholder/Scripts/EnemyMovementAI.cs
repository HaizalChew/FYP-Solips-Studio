using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform player;

    [Header("Values")]
    [SerializeField] private float maxAgroDistance;
    [SerializeField] private float fieldOfViewAngle = 60f, behindEnemyAngle = 150f, rotateSpeed = 1.0f;
    [SerializeField] private bool canMove = true;

    [SerializeField] private EnemyTypes enemyType;

    Vector3 dir;
    float speed;

    private void Start()
    {
        speed = agent.speed;
    }

    private void Update()
    {
        //find direction from player to enemy
        dir = player.position - transform.position;

        // Chase player if get too close to enemy
        if (Vector3.Distance(transform.position, player.position) < maxAgroDistance && IsInFieldOfView() && canMove)
        {
            // chase player
            agent.SetDestination(player.position);

        }

        if (Vector3.Distance(transform.position, player.position) < maxAgroDistance && !IsInFieldOfView() && canMove)
        {
            // Check if player is on the left or right
            float projectionOnRight = Vector3.Dot(dir, transform.right);

            //Debug.Log(projectionOnRight + " / " + Mathf.Abs(Vector3.Angle(transform.forward, dir)));    
            // Basically fine tunes the turning
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                if (projectionOnRight > 0 && Mathf.Abs(Vector3.Angle(transform.forward, dir)) < behindEnemyAngle)
                {
                    animator.SetTrigger("DoTurnRight");
                }
                else if (projectionOnRight < 0 && Mathf.Abs(Vector3.Angle(transform.forward, dir)) < behindEnemyAngle)
                {
                    animator.SetTrigger("DoTurnLeft");
                }
                // Skeleton Exclusive 
                else if (Mathf.Abs(Vector3.Angle(transform.forward, dir)) >= behindEnemyAngle)
                {
                    switch (enemyType)
                    {
                        case EnemyTypes.Skeleton:

                            animator.SetTrigger("DoTurnAround");
                            break;

                        case EnemyTypes.Werewolf:

                            if (projectionOnRight > 0)
                            {
                                animator.SetTrigger("DoTurnRight90");
                            }
                            else if (projectionOnRight < 0)
                            {
                                animator.SetTrigger("DoTurnLeft90");
                            }
                            break;
                    }
                    
                }
                else
                {
                    AutoRotate();
                }
            }            

        }

        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        canMove = !animator.GetCurrentAnimatorStateInfo(0).IsTag("Unmove") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Block") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("UnmoveNohead");
    }

    public bool IsInFieldOfView()
    {
        if (Mathf.Abs(Vector3.Angle(transform.forward, dir)) < fieldOfViewAngle)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }

    private void AutoRotate()
    {
        float singleStep = rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, dir, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
