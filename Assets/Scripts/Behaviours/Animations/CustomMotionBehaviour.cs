using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityAtoms.BaseAtoms;

public class CustomMotionBehaviour : StateMachineBehaviour
{
    private readonly int _isCustomID = Animator.StringToHash("IsCustom");

    private AnimatorStateInfo _animatorStateInfo;
    private AnimatorClipInfo _animatorClipInfo;

    private float _oldNormalizedTime;

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
        AnimatorBehaviour behaviour = animator.GetComponentInParent<AnimatorBehaviour>();
        behaviour.OnAnimationModifierExitRaised(new AnimatorModifierInfo(_animatorStateInfo, _animatorClipInfo));
    }
}
