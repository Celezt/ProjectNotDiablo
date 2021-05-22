using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[Serializable]
public struct AnimatorModifier : IEquatable<AnimatorModifier>
{
    public AnimationClip Clip
    {
        get => _clip;
        set => _clip = value;
    }
    public float SpeedMultiplier
    {
        get => _speedMultiplier;
        set => _speedMultiplier = value;
    }
    public float Exitpercent
    {
        get => _exitPercent;
        set => _exitPercent = value;
    }
    public Action<AnimatorModifierInfo> ExitAction { get => _exitAction; }
    public Action<AnimatorModifierInfo> EnterAction { get => _enterAction; }

    [SerializeField] private AnimationClip _clip;
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float _exitPercent;
    private Action<AnimatorModifierInfo> _enterAction;
    private Action<AnimatorModifierInfo> _exitAction;

    public bool Equals(AnimatorModifier other) => Clip.Equals(other.Clip);

    public AnimatorModifier(AnimationClip clip, float speedMultiplier = 1, float exitPercent = 1, Action<AnimatorModifierInfo> enterAction = null, Action<AnimatorModifierInfo> exitAction = null)
    {
        _clip = clip;
        _speedMultiplier = speedMultiplier;
        _exitPercent = exitPercent;
        _enterAction = enterAction;
        _exitAction = exitAction;
    }
}
