using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using MyBox;

public class DodgeBehaviour : MonoBehaviour
{
    public PlayerControls Controls
    {
        get => _controls;
    }

    #region Inspector
    [SerializeField] private MoveBehaviour _moveBehaviour;
    [Space(10)]
    [Header("Settings")]
    [SerializeField] private DodgeData[] _dodgeData;
    [Foldout("Atoms", true)]
    [SerializeField] private DurationValueList _invisibleFrameVariable;
    [SerializeField] private AnimatorModifierEvent _animatorModifierEvent;
    [SerializeField] private AnimatorModifierInfoEvent _animatorModifierInfoEvent;
    [SerializeField] private Vector3Variable _rawLocalInputMovement;

    #endregion

    [Serializable]
    private struct DodgeData
    {
        public AnimationClip Animation;
        [MinMaxRange(0, 180)] public RangedInt Angle;
        public float AnimationSpeedMultiplier;
        public float InvisibilityDuration;
        [Min(0)] public float Cooldown;
        public float ForceStrength;
        public AnimationCurve ForceCurve;
        public Vector3 Direction;
    }

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

                Vector3 rawValue = _rawLocalInputMovement.Value;
                _angle = Vector3.Angle(Vector3.forward, (rawValue != Vector3.zero) ? rawValue.normalized : Vector3.forward);

                foreach (DodgeData data in _dodgeData)
                {
                    if (_angle >= data.Angle.Min && _angle <= data.Angle.Max)
                    {
                        _invisibleFrameVariable.Add(new Duration(data.InvisibilityDuration));

                        AnimatorModifier modifier = new AnimatorModifier(data.Animation, data.AnimationSpeedMultiplier);
                        _animatorModifierEvent.Raise(modifier);

                        StartCoroutine(DodgeLerp(data.Direction, data.Animation.length, data.AnimationSpeedMultiplier, data.ForceStrength, data.ForceCurve));

                        break;
                    }
                }

                _controls.Disable();
            }
        }
    }

    public void OnExitDodge(AnimatorModifierInfo animatorInfo)
    {
        foreach (DodgeData data in _dodgeData)
        {
            if (_angle >= data.Angle.Min && _angle <= data.Angle.Max)
            {
                _afteRollCooldown = new Duration(data.Cooldown);

                break;
            }
        }

        _controls.Enable();

        if (_moveBehaviour != null)
        {
            _moveBehaviour.Controls.Enable();
            _moveBehaviour.IsRotating = true;
            _moveBehaviour.SmoothInputMovement = Vector3.zero;
            _rigidbody.velocity /= 4;
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
