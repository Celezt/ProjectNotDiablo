using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[Serializable]
public struct AnimatorModifier : IEquatable<AnimatorModifier>
{
    public AnimationClip Clip { get => _clip; set => _clip = value; }
    public float SpeedMultiplier { get => _speedMultiplier; set => _speedMultiplier = value; }
    public Action<AnimatorModifierInfo> ExitAction { get => _endAction; }

    [SerializeField] private AnimationClip _clip;
    [SerializeField] private float _speedMultiplier;
    private Action<AnimatorModifierInfo> _endAction;

    public bool Equals(AnimatorModifier other) => Clip.Equals(other.Clip);

    public AnimatorModifier(AnimationClip clip, float speedMultiplier = 1, Action<AnimatorModifierInfo> exitAction = null)
    {
        _clip = clip;
        _speedMultiplier = speedMultiplier;
        _endAction = exitAction;
    }
}
