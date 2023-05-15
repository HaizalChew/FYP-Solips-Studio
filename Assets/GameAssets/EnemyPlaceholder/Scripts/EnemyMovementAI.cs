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
    [SerializeField] private float fieldOfViewAngle;

    private void Update()
    {
        // Chase player if get too close to enemy
        if (Vector3.Distance(transform.position, player.position) < maxAgroDistance && IsInFieldOfView())
        {
            // chase player
            agent.SetDestination(player.position);

        }

        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    private bool IsInFieldOfView()
    {
        Vector3 dir = player.position - transform.position;
        if (Mathf.Abs(Vector3.Angle(transform.forward, dir)) < fieldOfViewAngle)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }
}
