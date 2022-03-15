using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Hurt : StateMachineBehaviour
{
    Boss_Health bossH;
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossH = animator.GetComponent<Boss_Health>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossH.BossHurt();

        if(bossH.damagetaken == true)
        {
            animator.SetTrigger("Hurt");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Hurt");
    }

}
