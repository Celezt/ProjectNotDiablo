using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[Serializable]
public struct AnimatorModifierInfo : IEquatable<AnimatorModifierInfo>
{
    public AnimatorControllerPlayable Controller { get; }
    public AnimatorStateInfo StateInfo { get; }

    public bool Equals(AnimatorModifierInfo other) => Controller.Equals(other.Controller) && StateInfo.Equals(StateInfo);

    public AnimatorModifierInfo(AnimatorControllerPlayable controller, AnimatorStateInfo stateInfo)
    {
        Controller = controller;
        StateInfo = stateInfo;
    }
}
