using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityAtoms;
using MyBox;

[EditorIcon("atom-icon-sand")]
[CreateAssetMenu(menuName = "Unity Atoms/Functions/Custom/Invisibility Frame Float", fileName = "InvisibilityFrameFloat")]
public class InvisibilityFrameFloat : FloatFloatFunction
{
    [Tooltip("Duration of the invisibility frame.")]
    [SerializeField, MustBeAssigned] private DurationValueList _durationValueList;
    [Tooltip("What invisibility frame should affect.")]
    [SerializeField] private AffectType _affect;

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

    public override float Call(float t1)
    {
        if (Application.isPlaying)
        {
            if (_isInit)
            {
                _oldValue = t1;
                _isInit = false;
            }

            _durationValueList.List.RemoveAll(item => !item.IsActive);

            if (_durationValueList.Count > 0)
            {
                switch (_affect)
                {
                    case AffectType.Both:
                        return _oldValue;
                    case AffectType.Positive:
                        return t1 < _oldValue ? _oldValue = t1 : _oldValue;
                    case AffectType.Negative:
                        return t1 > _oldValue ? _oldValue = t1 : _oldValue;
                    default:
                        throw new UnityException(_affect.ToString() + " is not supported");
                }
            }
            else
                return _oldValue = t1;
        }

        return t1;
    }
}
