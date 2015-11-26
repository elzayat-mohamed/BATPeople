using UnityEngine;
using System.Collections;
using System;

public class AnimationStateEventDelegator : StateMachineBehaviour 
{

    public event Action OnEnter;
    public event Action OnExit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (OnEnter != null)
        {
            OnEnter();
        }
   

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (OnExit != null)
        {
            OnExit();
        }
        base.OnStateExit(animator, stateInfo, layerIndex);
    
    }

    
}
