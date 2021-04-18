using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AnimatorModifier : IEquatable<AnimatorModifier>
{
    public AnimationClip Clip { get => _clip; set => _clip = value; }
    public float SpeedMultiplier { get => _speedMultiplier; set => _speedMultiplier = value; }

    [SerializeField] private AnimationClip _clip;
    [SerializeField] private float _speedMultiplier;

    public bool Equals(AnimatorModifier other) => Clip.Equals(other.Clip);

    public AnimatorModifier(AnimationClip clip, float speedMultiplier = 1)
    {
        _clip = clip;
        _speedMultiplier = speedMultiplier;
    }
}
