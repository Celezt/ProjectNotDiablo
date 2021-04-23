using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

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

    public void RaiseAnimatorModifier(AnimatorModifier modifier) => OnAnimationModifierRaised(modifier);

    #region Inspector
    [Header("Atoms")]
    [SerializeField] private Vector3Reference _smoothLocalMotionReference;
    [SerializeField] private AnimatorModifierEventReference _animatorModifierEvent;
    [SerializeField] private BoolReference _fallingReference;
    [Space(10)]
    [Header("Animations Settings")]
    [SerializeField] private Animator _animator;
    #endregion

    private AnimatorOverrideController _animatorOverrideController;

    private readonly int _motionZID = Animator.StringToHash("MotionZ");
    private readonly int _motionXID = Animator.StringToHash("MotionX");
    private readonly int _isFallingID = Animator.StringToHash("IsFalling");
    private readonly int _isCustomID = Animator.StringToHash("IsCustom");
    private readonly int _isWalkingID = Animator.StringToHash("IsWalking");
    private readonly int _customMotionSpeedID = Animator.StringToHash("CustomMotionSpeed");

    #region Events
    public void OnAnimationModifierRaised(AnimatorModifier value)
    {
        _animatorOverrideController["Empty Custom Motion"] = value.Clip;
        _animator.SetFloat(_customMotionSpeedID, value.SpeedMultiplier);
        _animator.SetBool(_isCustomID, true);
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
        if (_smoothLocalMotionReference != null)
        {
            Vector3 blend = _smoothLocalMotionReference.Value;
            _animator.SetFloat(_motionZID, blend.z);
            _animator.SetFloat(_motionXID, blend.x);
            _animator.SetBool(_isWalkingID, blend.magnitude > 0.1f);
        }
    }

    private void UpdateFalling()
    {
        if (_fallingReference.Usage <= 1)   // If no event is used.
            _animator.SetBool(_isFallingID, _fallingReference.Value);
    }
}
