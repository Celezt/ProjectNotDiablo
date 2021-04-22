using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;

public class RollBehaviour : MonoBehaviour
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
    [Space(10)]
    [Header("Settings")]
    [SerializeField] private AnimationClip _animation;
    [SerializeField] private float _animationSpeedMultiplier = 1.0f;
    [SerializeField] private float _invisibilityDuration = 1.0f;
    [SerializeField] private float _cooldown = 0.5f;
    [SerializeField] private float _forceStrength = 50.0f;
    [SerializeField] private AnimationCurve _forceCurve = AnimationCurve.Linear(0.0f, 0.5f, 1.0f, 0.5f);

    #endregion

    private PlayerControls _controls;
    private Rigidbody _rigidbody;

    private Duration _afteRollCooldown;

    #region Events
    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_afteRollCooldown.IsActive)
            {
                _invisibleFrameVariable.Add(new Duration(_invisibilityDuration));

                AnimatorModifier modifier = new AnimatorModifier(_animation, _animationSpeedMultiplier);
                modifier.Clip = _animation;
                _animatorModifierEvent.Raise(modifier);

                _controls.Disable();

                if (_moveBehaviour != null)
                {
                    _moveBehaviour.Controls.Disable();
                    _moveBehaviour.IsRotating = false;
                }

                StartCoroutine(RollLerp());
            }
        }
    }

    public void OnExitRoll(AnimatorModifierInfo animatorInfo)
    {
        _afteRollCooldown = new Duration(_cooldown);
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
        _controls.Ground.Roll.performed += OnRoll;
        _controls.Ground.Roll.canceled += OnRoll;
        _animatorModifierInfoEvent.Register(OnExitRoll);
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Ground.Roll.performed -= OnRoll;
        _controls.Ground.Roll.canceled -= OnRoll;
        _animatorModifierInfoEvent.Unregister(OnExitRoll);
        _controls.Disable();
    }
    #endregion

    private IEnumerator RollLerp()
    {
        float length = _animation.length * _animationSpeedMultiplier;
        float delta = 1 / length;
        float timer = 0;

        while (timer < 1)
        {
            timer += delta * Time.fixedDeltaTime;
            _rigidbody?.AddRelativeForce(Vector3.forward * (_rigidbody?.mass ?? 1.0f) * _forceCurve.Evaluate(timer) * _forceStrength, ForceMode.Force);

            yield return new WaitForFixedUpdate();
        }
    }
}
