using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using MyBox;

public class AnimatorBehaviour : MonoBehaviour
{
    public Vector3 SmoothLocalMotion
    {
        get => _smoothLocalMotionReference.Value;
        set => _smoothLocalMotionReference.Value = value;
    }

    public bool IsFalling
    {
        get => _fallingReference.Value;
        set => _fallingReference.Value = value;
    }

    public Animator Animator => _animator;

    public bool IsAnimationModifierRunning { get => _isAnimationModifierRunning; }

    #region Inspector
    [Header("Settings")]
    [SerializeField, MustBeAssigned] private Animator _animator;
    [Foldout("Atoms", true)]
    [SerializeField] private Vector3Reference _smoothLocalMotionReference = new Vector3Reference();
    [SerializeField] private AnimatorModifierEventReference _animatorModifierEvent = new AnimatorModifierEventReference();
    [SerializeField] private BoolReference _fallingReference = new BoolReference();
    #endregion

    private AnimatorOverrideController _animatorOverrideController;
    private Queue<Action<AnimatorModifierInfo>> _exitCustomActionQueue;

    private int _customIndex;
    private bool _isAnimationModifierRunning;

    private readonly int _motionZID = Animator.StringToHash("MotionZ");
    private readonly int _motionXID = Animator.StringToHash("MotionX");
    private readonly int _motionMagnitudeID = Animator.StringToHash("MotionMagnitude");
    private readonly int _isFallingID = Animator.StringToHash("IsFalling");
    private readonly int _isCustomID = Animator.StringToHash("IsCustom");
    private readonly int _customIndexID = Animator.StringToHash("CustomIndex");
    private readonly int _customMotionSpeedID = Animator.StringToHash("CustomMotionSpeed");

    #region Events
    /// <summary>
    /// Raise a new custom animation.
    /// </summary>
    public void OnAnimationModifierRaised(AnimatorModifier value)
    {
        _isAnimationModifierRunning = true;

        switch (_customIndex)
        {
            case 0:
                _animatorOverrideController["Empty Custom Motion 0"] = value.Clip;
                break;
            case 1:
                _animatorOverrideController["Empty Custom Motion 1"] = value.Clip;
                break;
        }

        _animator.SetInteger(_customIndexID, _customIndex);
        _animator.SetFloat(_customMotionSpeedID, value.SpeedMultiplier);
        _animator.SetBool(_isCustomID, true);

        _exitCustomActionQueue.Enqueue(value.ExitAction);

        _customIndex = (_customIndex + 1) % 2;
    }

    /// <summary>
    /// WARNING: should not be called from outside.
    /// </summary>
    public void OnAnimationModifierExitRaised(AnimatorModifierInfo info)
    {
        _isAnimationModifierRunning = false;

        _exitCustomActionQueue.Dequeue()?.Invoke(info);
    }

    public void OnFalling(bool value)
    {
        _animator.SetBool(_isFallingID, value);
    }
    #endregion

    #region Unity Message
    private void Start()
    {
        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animatorOverrideController;

        _exitCustomActionQueue = new Queue<Action<AnimatorModifierInfo>>();
    }

    private void OnEnable()
    {
        _animatorModifierEvent?.Event?.Register(OnAnimationModifierRaised);

        if (_fallingReference.Usage >= 2)
            _fallingReference.GetEvent<BoolEvent>().Register(OnFalling);
    }

    private void Update()
    {
        UpdateFalling();
        UpdateMotionAnimation();
    }

    private void OnDisable()
    {
        _animatorModifierEvent?.Event?.Unregister(OnAnimationModifierRaised);

        if (_fallingReference.Usage >= 2)
            _fallingReference.GetEvent<BoolEvent>().Unregister(OnFalling);
    }
    #endregion

    private void UpdateMotionAnimation()
    {
        Vector3 blend = _smoothLocalMotionReference.Value;
        _animator.SetFloat(_motionZID, blend.z);
        _animator.SetFloat(_motionXID, blend.x);
        _animator.SetFloat(_motionMagnitudeID, blend.magnitude);
    }

    private void UpdateFalling()
    {
        if (_fallingReference.Usage <= 1)   // If no event is used.
            _animator.SetBool(_isFallingID, _fallingReference.Value);
    }
}
