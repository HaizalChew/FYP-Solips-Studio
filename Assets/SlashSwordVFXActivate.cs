using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSwordVFXActivate : StateMachineBehaviour
{
    [SerializeField] private ParticleSystem swordGlowVfx;
    [SerializeField] private CurrentEntity currentEntity;
    

    enum CurrentEntity
    {
        Player,
        Aldric,
        Werewolf
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (swordGlowVfx == null)
        {
            switch (currentEntity)
            {
                case CurrentEntity.Player:
                    swordGlowVfx = GameObject.FindGameObjectWithTag("PlayerGlowVFX").GetComponent<ParticleSystem>();
                    break;

                case CurrentEntity.Aldric:
                    swordGlowVfx = GameObject.FindGameObjectWithTag("AldricGlowVFX").GetComponent<ParticleSystem>();
                    break;

                case CurrentEntity.Werewolf:
                    swordGlowVfx = GameObject.FindGameObjectWithTag("WerewolfGlowVFX").GetComponent<ParticleSystem>();
                    break;

            }

            swordGlowVfx.Play(true);
        }
        else
        {
            swordGlowVfx.Play(true);
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        if (swordGlowVfx == null)
        {
            switch (currentEntity)
            {
                case CurrentEntity.Player:
                    swordGlowVfx = GameObject.FindGameObjectWithTag("PlayerGlowVFX").GetComponent<ParticleSystem>();
                    break;

                case CurrentEntity.Aldric:
                    swordGlowVfx = GameObject.FindGameObjectWithTag("AldricGlowVFX").GetComponent<ParticleSystem>();
                    break;

                case CurrentEntity.Werewolf:
                    swordGlowVfx = GameObject.FindGameObjectWithTag("WerewolfGlowVFX").GetComponent<ParticleSystem>();
                    break;

            }

            swordGlowVfx.Clear(true);
        }
        else
        {
            swordGlowVfx.Clear(true);
        }
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
