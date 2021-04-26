using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityAtoms.BaseAtoms;

public class CustomMotionBehaviour : StateMachineBehaviour
{
    [SerializeField] private AnimatorModifierInfoEvent _animatorModifierInfoEvent;

    private readonly int _isCustomID = Animator.StringToHash("IsCustom");

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_isCustomID, false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        _animatorModifierInfoEvent.Raise(new AnimatorModifierInfo(controller, stateInfo));
    }
}
