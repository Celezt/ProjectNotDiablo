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

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        //AnimatorStateInfo info = controller.GetCurrentAnimatorStateInfo(layerIndex);
        //Debug.Log(controller.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime);
        //if (controller.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= 1.0f)
        //{
        //   // Debug.Log(controller.GetCurrentAnimatorClipInfo(layerIndex)[0].clip.name);
        //}
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        _animatorModifierInfoEvent.Raise(new AnimatorModifierInfo(controller, stateInfo));
    }
}
