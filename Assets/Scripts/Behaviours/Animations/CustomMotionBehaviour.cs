using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityAtoms.BaseAtoms;

public class CustomMotionBehaviour : StateMachineBehaviour
{
    [SerializeField] private AnimatorModifierInfoEvent _animatorModifierInfoEvent;

    private readonly int _isCustomID = Animator.StringToHash("IsCustom");
    private readonly int hash = Animator.StringToHash("Custom Motion");

    private AnimatorStateInfo _animatorStateInfo;
    private AnimatorClipInfo _animatorClipInfo;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_isCustomID, false);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        _animatorClipInfo = controller.GetCurrentAnimatorClipInfo(layerIndex)[0];
        _animatorStateInfo = stateInfo;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        _animatorModifierInfoEvent?.Raise(new AnimatorModifierInfo(_animatorStateInfo, _animatorClipInfo));
    }
}
