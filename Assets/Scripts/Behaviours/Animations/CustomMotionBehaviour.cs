using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityAtoms.BaseAtoms;

public class CustomMotionBehaviour : StateMachineBehaviour
{
    private readonly int _isCustomID = Animator.StringToHash("IsCustom");
    private readonly int _exitPercentID = Animator.StringToHash("ExitPercent");

    private AnimatorStateInfo _animatorStateInfo;

    private float _exitPercent;
    private bool _isRaised;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _isRaised = false;
        animator.SetBool(_isCustomID, false);
        _exitPercent = animator.GetFloat(_exitPercentID);

        AnimatorBehaviour animatorBehaviour = animator.GetComponentInParent<AnimatorBehaviour>();
        animatorBehaviour.Internal.OnAnimatorModifierEnterRaised(new AnimatorModifierInfo(_animatorStateInfo, animatorBehaviour, animator));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animatorStateInfo = stateInfo;

        if (!_isRaised && stateInfo.normalizedTime >= _exitPercent)
        {
            _isRaised = true;
            AnimatorBehaviour animatorBehaviour = animator.GetComponentInParent<AnimatorBehaviour>();
            animatorBehaviour.Internal.OnAnimationModifierExitRaised(new AnimatorModifierInfo(_animatorStateInfo, animatorBehaviour, animator), false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AnimatorBehaviour animatorBehaviour = animator.GetComponentInParent<AnimatorBehaviour>();
        if (!_isRaised)
        {
            animatorBehaviour.Internal.OnAnimationModifierExitRaised(new AnimatorModifierInfo(_animatorStateInfo, animatorBehaviour, animator), true);
        }

        _isRaised = false;
    }
}
