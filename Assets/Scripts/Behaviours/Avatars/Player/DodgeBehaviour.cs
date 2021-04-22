using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;

public class DodgeBehaviour : MonoBehaviour
{
    public PlayerControls Controls
    {
        get => _controls;
    }

    #region Inspector
    [SerializeField] private MoveBehaviour _moveBehaviour;
    [Space(10)]
    [Header("Atoms")]
    [SerializeField] private DurationValueList _invisibleFrameVariable;
    [SerializeField] private AnimatorModifierEvent _animatorModifierEvent;
    [SerializeField] private AnimatorModifierInfoEvent _animatorModifierInfoEvent;
    [SerializeField] private Vector3Variable _smoothLocalInputMovement;
    [Space(10)]
    [Header("Settings")]
    [SerializeField] private AnimationClip _forwardAnimation;
    [SerializeField] private float _forwardsAnimationSpeedMultiplier = 1.0f;
    [SerializeField] private float _forwardsInvisibilityDuration = 1.0f;
    [SerializeField] private float _forwardsCooldown = 0.5f;
    [SerializeField] private float _forwardsForceStrength = 50.0f;
    [SerializeField] private AnimationCurve _forwardsforceCurve = AnimationCurve.Linear(0.0f, 0.5f, 1.0f, 0.5f);
    [Space(10)]
    [SerializeField] private AnimationClip _backwardsAnimation;
    [SerializeField] private float _backwardsAnimationSpeedMultiplier = 1.0f;
    [SerializeField] private float _backwardsInvisibilityDuration = 1.0f;
    [SerializeField] private float _backwardsCooldown = 0.5f;
    [SerializeField] private float _backwardsForceStrength = 50.0f;
    [SerializeField] private AnimationCurve _backwarsForceCurve = AnimationCurve.Linear(0.0f, 0.5f, 1.0f, 0.5f);

    #endregion

    private PlayerControls _controls;
    private Rigidbody _rigidbody;

    private float _angle;

    private Duration _afteRollCooldown;

    #region Events
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_afteRollCooldown.IsActive)
            {
                if (_moveBehaviour != null)
                {
                    if (_moveBehaviour.IsFalling)   // Don't roll if falling.
                        return;

                    _moveBehaviour.Controls.Disable();
                    _moveBehaviour.IsRotating = false;
                }

                Vector3 _inputValue = _smoothLocalInputMovement.Value;
                _angle = Vector3.Angle(Vector3.forward, (_inputValue.magnitude > 0.1f) ? _inputValue.normalized : Vector3.forward);

                if (_angle >= 150)
                {
                    _invisibleFrameVariable.Add(new Duration(_backwardsInvisibilityDuration));

                    AnimatorModifier modifier = new AnimatorModifier(_backwardsAnimation, _backwardsAnimationSpeedMultiplier);
                    _animatorModifierEvent.Raise(modifier);

                    StartCoroutine(DodgeLerp(Vector3.back, _backwardsAnimation.length, _backwardsAnimationSpeedMultiplier, _backwardsForceStrength, _backwarsForceCurve));
                }
                else
                {
                    _invisibleFrameVariable.Add(new Duration(_forwardsInvisibilityDuration));

                    AnimatorModifier modifier = new AnimatorModifier(_forwardAnimation, _forwardsAnimationSpeedMultiplier);
                    _animatorModifierEvent.Raise(modifier);

                    StartCoroutine(DodgeLerp(Vector3.forward, _forwardAnimation.length, _forwardsAnimationSpeedMultiplier, _forwardsForceStrength, _forwardsforceCurve));
                }

                _controls.Disable();
            }
        }
    }

    public void OnExitDodge(AnimatorModifierInfo animatorInfo)
    {
        if (_angle >= 150)
        {
            _afteRollCooldown = new Duration(_backwardsCooldown);
        }
        else
        {
            _afteRollCooldown = new Duration(_forwardsCooldown);
        }

        _controls.Enable();

        if (_moveBehaviour != null)
        {
            _moveBehaviour.Controls.Enable();
            _moveBehaviour.IsRotating = true;
        }
    }
    #endregion

    #region Unity Message
    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _controls.Ground.Roll.performed += OnDodge;
        _controls.Ground.Roll.canceled += OnDodge;
        _animatorModifierInfoEvent.Register(OnExitDodge);
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Ground.Roll.performed -= OnDodge;
        _controls.Ground.Roll.canceled -= OnDodge;
        _animatorModifierInfoEvent.Unregister(OnExitDodge);
        _controls.Disable();
    }
    #endregion

    private IEnumerator DodgeLerp(Vector3 direction, float length, float speedMultiplier, float strength, AnimationCurve curve)
    {
        float duration = length * speedMultiplier;
        float delta = 1 / duration;
        float timer = 0;

        while (timer < 1)
        {
            timer += delta * Time.fixedDeltaTime;
            _rigidbody?.AddRelativeForce(direction * (_rigidbody?.mass ?? 1.0f) * curve.Evaluate(timer) * strength, ForceMode.Force);

            yield return new WaitForFixedUpdate();
        }
    }
}
