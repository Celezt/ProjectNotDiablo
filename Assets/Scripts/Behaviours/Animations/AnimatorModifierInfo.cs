using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[Serializable]
public struct AnimatorModifierInfo : IEquatable<AnimatorModifierInfo>
{
    public AnimatorStateInfo StateInfo { get; }
    public Animator Animator { get; }
    public AnimatorBehaviour AnimatorBehaviour { get; }

    public bool Equals(AnimatorModifierInfo other) => StateInfo.Equals(other.StateInfo);

    public AnimatorModifierInfo(AnimatorStateInfo stateInfo, AnimatorBehaviour animatorBehaviour, Animator animator)
    {
        StateInfo = stateInfo;
        AnimatorBehaviour = animatorBehaviour;
        Animator = animator;
    }
}
