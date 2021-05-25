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

    public bool EnableCustomAnimation
    {
        get => _enableCustomAnimation;
        set => _enableCustomAnimation = value;
    }

    public bool IsAnimationModifierRunning => _isAnimationModifierRunning;

    public void SetMotionSpeed(float value) => _animator.SetFloat(_customMotionSpeedID, value);
    public void RaiseDying() => _dyingReference.Event.Raise();

    public InternalBehaviour Internal => _internal;

    #region Inspector
    [Header("Settings")]
    [SerializeField, MustBeAssigned] private Animator _animator;
    [Header("Atoms")]
    [SerializeField] private Vector3Reference _smoothLocalMotionReference = new Vector3Reference();
    [SerializeField] private AnimatorModifierEventReference _animatorModifierEvent = new AnimatorModifierEventReference();
    [SerializeField] private BoolReference _fallingReference = new BoolReference();
    [SerializeField] private VoidBaseEventReference _dyingReference;
    #endregion

    private AnimatorOverrideController _animatorOverrideController;
    private Queue<Action<AnimatorModifierInfo>> _exitCustomActionQueue = new Queue<Action<AnimatorModifierInfo>>();
    private Action<AnimatorModifierInfo> _enterCustomAction;

    private InternalBehaviour _internal;

    private int _customIndex;
    private bool _isAnimationModifierRunning;
    private bool _enableCustomAnimation = true;

    private readonly int _motionZID = Animator.StringToHash("MotionZ");
    private readonly int _motionXID = Animator.StringToHash("MotionX");
    private readonly int _motionMagnitudeID = Animator.StringToHash("MotionMagnitude");
    private readonly int _isFallingID = Animator.StringToHash("IsFalling");
    private readonly int _isCustomID = Animator.StringToHash("IsCustom");
    private readonly int _customIndexID = Animator.StringToHash("CustomIndex");
    private readonly int _customMotionSpeedID = Animator.StringToHash("CustomMotionSpeed");
    private readonly int _exitPercentID = Animator.StringToHash("ExitPercent");
    private readonly int _isDyingID = Animator.StringToHash("IsDying");
    #region Events

    /// <summary>
    /// Raise a new custom animation.
    /// </summary>
    public void OnAnimationModifierRaised(AnimatorModifier value)
    {
        if (_enableCustomAnimation)
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
            _animator.SetFloat(_exitPercentID, value.Exitpercent);
            _animator.SetFloat(_customMotionSpeedID, value.SpeedMultiplier);
            _animator.SetBool(_isCustomID, true);

            _exitCustomActionQueue.Enqueue(value.ExitAction);
            _enterCustomAction = value.EnterAction;

            _customIndex = (_customIndex + 1) % 2;
        }
    }

    public void OnFalling(bool value) => _animator.SetBool(_isFallingID, value);
    public void OnDying() => _animator.SetBool(_isDyingID, true);
    #endregion

    #region Unity Message
    private void Awake()
    {
        _internal = new InternalBehaviour(this);
    }

    private void Start()
    {
        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animatorOverrideController;
    }

    private void OnEnable()
    {
        _animatorModifierEvent?.Event?.Register(OnAnimationModifierRaised);

        if (_fallingReference.Usage >= 2)
            _fallingReference.GetEvent<BoolEvent>().Register(OnFalling);

        if (_dyingReference != null)
            _dyingReference.Event?.Register(OnDying);
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

        if (_dyingReference != null)
            _dyingReference.Event?.Unregister(OnDying);
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

    /// <summary>
    /// WARNING: should not be called from outside.´å
    /// </summary>
    public class InternalBehaviour
    {
        private AnimatorBehaviour _animatorBehaviour;

        public InternalBehaviour(AnimatorBehaviour animatorBehaviour) => _animatorBehaviour = animatorBehaviour;

        public void OnAnimatorModifierEnterRaised(AnimatorModifierInfo info)
        {
            _animatorBehaviour._enterCustomAction?.Invoke(info);
            _animatorBehaviour._enterCustomAction = null;
        }

        public void OnAnimationModifierExitRaised(AnimatorModifierInfo info, bool isLastExit)
        {
            if (isLastExit)
            {
                for (int i = 0; i < _animatorBehaviour._exitCustomActionQueue.Count; i++)
                {
                    _animatorBehaviour._exitCustomActionQueue.Dequeue()?.Invoke(info);
                }
            }
            else
            {
                if (_animatorBehaviour._exitCustomActionQueue.Count > 0)
                    _animatorBehaviour._exitCustomActionQueue.Dequeue()?.Invoke(info);
            }

            _animatorBehaviour._isAnimationModifierRunning = (_animatorBehaviour._exitCustomActionQueue.Count > 0);
        }
    }
}
