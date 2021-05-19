using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[Serializable]
public struct AnimatorModifierInfo : IEquatable<AnimatorModifierInfo>
{
    public AnimatorStateInfo StateInfo { get; }
    public AnimatorClipInfo ClipInfo { get; }

    public bool Equals(AnimatorModifierInfo other) => StateInfo.Equals(other.StateInfo);

    public AnimatorModifierInfo(AnimatorStateInfo stateInfo, AnimatorClipInfo clipInfo)
    {
        StateInfo = stateInfo;
        ClipInfo = clipInfo;
    }
}
