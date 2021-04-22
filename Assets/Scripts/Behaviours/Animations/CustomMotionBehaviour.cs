using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityAtoms.BaseAtoms;

public class CustomMotionBehaviour : StateMachineBehaviour
{
    [SerializeField] private AnimatorModifierInfoEvent _animatorModifierInfoEvent;

    private readonly int _isCustomID = Animator.StringToHash("IsCustom");

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        animator.SetBool(_isCustomID, false);
        _animatorModifierInfoEvent.Raise(new AnimatorModifierInfo(controller, stateInfo));
    }
}
