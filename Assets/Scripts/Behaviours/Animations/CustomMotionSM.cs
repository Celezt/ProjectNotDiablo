using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityAtoms.BaseAtoms;

public class CustomMotionSM : StateMachineBehaviour
{
    [Header("Atoms")]
    [SerializeField] private AnimatorModifierInfoEvent _animatorModifierInfoEvent;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        _animatorModifierInfoEvent.Raise(new AnimatorModifierInfo(controller, stateInfo));
    }
}
