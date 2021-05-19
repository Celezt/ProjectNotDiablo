using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class AnimationCustomEvent : MonoBehaviour
{
    [SerializeField, MustBeAssigned] private AnimatorBehaviour _animatorBehaviour;

    private Animator _animator;

    public void RaiseExit()
    {
        _animatorBehaviour.OnAnimationModifierExitRaised(new AnimatorModifierInfo(_animator.GetCurrentAnimatorStateInfo(0), _animator.GetCurrentAnimatorClipInfo(0)[0]));
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
}
