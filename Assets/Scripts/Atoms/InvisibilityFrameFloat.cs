using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityAtoms;

[EditorIcon("atom-icon-sand")]
[CreateAssetMenu(menuName = "Unity Atoms/Functions/Custom/Invisibility Frame Float", fileName = "InvisibilityFrameFloat")]
public class InvisibilityFrameFloat : FloatFloatFunction
{
    [Tooltip("Duration of the invisibility frame.")]
    public DurationValueList Duration;
    [Tooltip("What invisibility frame should affect.")]
    public AffectType Affect;

    private float _oldValue;
    private bool _isInit;

    public enum AffectType
    {
        Both,
        Positive,
        Negative,
    }

    private void OnEnable()
    {
        _isInit = true;
    }

    public override float Call(float value)
    {
        if (_isInit)
        {
            _oldValue = value;
            _isInit = false;
        }

        for (int i = 0; i < Duration.Count; i++)
        {
            Duration invisibleFrame = Duration[i];

            if (!invisibleFrame.IsActive)
                Duration.Remove(invisibleFrame);
        }

        if (Duration.Count != 0)
        {
            switch (Affect)
            {
                case AffectType.Both:
                    return _oldValue;
                case AffectType.Positive:
                    return value < _oldValue ? _oldValue = value : _oldValue;
                case AffectType.Negative:
                    return value > _oldValue ? _oldValue = value : _oldValue;
                default:
                    throw new UnityException(Affect.ToString() + " is not supported");
            }
        }
        else
            return _oldValue = value;
    }
}
