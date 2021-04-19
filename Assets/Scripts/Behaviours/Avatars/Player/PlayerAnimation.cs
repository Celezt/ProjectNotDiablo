using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class PlayerAnimation : MonoBehaviour
{
    #region Inspector
    [Header("Atoms")]
    [SerializeField] private Vector3Variable _smoothLocalInputMovementVariable;
    [SerializeField] private AnimatorModifierEvent _animatorModifierEvent;
    [Space(10)]
    [Header("Animations Settings")]
    [SerializeField] private Animator _animator;
    [Space(10)]
    public float IdleBlendSpeed = 0.002f;
    public float MotionBlendMargin = 0.01f;
    #endregion

    private AnimatorOverrideController _animatorOverrideController;

    private float _idleBlend;

    private readonly int _motionZID = Animator.StringToHash("MotionZ");
    private readonly int _motionXID = Animator.StringToHash("MotionX");
    private readonly int _idleID = Animator.StringToHash("IdleBlend");

    #region Events
    public void OnAnimationModifierRaised(AnimatorModifier value)
    {
        _animatorOverrideController["Empty Custom Motion"] = value.Clip;
        _animator.SetFloat("CustomMotionSpeed", value.SpeedMultiplier);
        _animator.Play("Custom_Motion");
    }
    #endregion

    #region Unity Message
    private void Start()
    {
        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animatorOverrideController;
    }

    private void OnEnable()
    {
        _animatorModifierEvent.Register(OnAnimationModifierRaised);
    }

    private void Update()
    {
        UpdateMotionAnimation();
        UpdateIdleAnimation();
    }

    private void OnDisable()
    {
        _animatorModifierEvent.Unregister(OnAnimationModifierRaised);
    }
    #endregion

    private void UpdateMotionAnimation()
    {
        if (_smoothLocalInputMovementVariable != null)
        {
            Vector3 blend = _smoothLocalInputMovementVariable.Value;
            _animator.SetFloat(_motionZID, blend.z);
            _animator.SetFloat(_motionXID, blend.x);
        }
    }

    private void UpdateIdleAnimation()
    {
        _idleBlend = (_idleBlend + IdleBlendSpeed) % 1;
        _animator.SetFloat(_idleID, _idleBlend);
    }
}
