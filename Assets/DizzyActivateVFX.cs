using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DizzyActivateVFX : StateMachineBehaviour
{
    public GameObject dizzyVFX;
    public GameObject vFX;
    public Transform playerDizzySpawnPoint;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerDizzySpawnPoint = GameObject.FindGameObjectWithTag("StunVFX").GetComponent<Transform>();

        vFX = Instantiate(dizzyVFX, playerDizzySpawnPoint.position, Quaternion.identity);
        vFX.GetComponent<ParticleSystem>().Play(true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        vFX.transform.position = playerDizzySpawnPoint.position;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        vFX.GetComponent<ParticleSystem>().Stop(true);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
