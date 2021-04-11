using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class PlayerAnimation : MonoBehaviour
{
    #region Inspector
    [Header("Setup")]
    [SerializeField] private Animator _animation;
    [Space(10)]
    [SerializeField] private Vector3Variable _smoothInputMovementAtoms;
    [SerializeField] private BoolVariable _isDashingAtoms;
    [Space(10)]
    [Header("Settings")]
    public float IdleBlendSpeed = 0.002f;
    public float MotionBlendMargin = 0.01f;
    #endregion

    private ParameterID _id;

    private float _idleBlend;

    private readonly struct ParameterID
    {
        public readonly int Motion;
        public readonly int Idle;
        public readonly int Dash;

        public ParameterID(int motion, int idle, int dash)
        {
            Motion = motion;
            Idle = idle;
            Dash = dash;
        }
    }

    #region Events
    public void OnDashChange(bool value) => _animation.SetBool(_id.Dash, value);
    #endregion

    #region Unity Message
    private void Start()
    {
        _id = new ParameterID(
            Animator.StringToHash("Velocity"),
            Animator.StringToHash("Idle"),
            Animator.StringToHash("IsDashing")
        );

        _isDashingAtoms.Changed.Register(OnDashChange);
    }

    private void Update()
    {
        UpdateMotionAnimation();
        UpdateIdleAnimation();
    }

    private void OnDestroy()
    {
        _isDashingAtoms.Changed.Unregister(OnDashChange);
    }
    #endregion

    private void UpdateMotionAnimation()
    {
        if (_smoothInputMovementAtoms != null)
        {
            float blend = _smoothInputMovementAtoms.Value.magnitude;
            _animation.SetFloat(_id.Motion, (blend > 0.01f) ? blend : 0.0f);
        }
    }

    private void UpdateIdleAnimation()
    {
        _idleBlend = (_idleBlend + IdleBlendSpeed) % 1;
        _animation.SetFloat(_id.Idle, _idleBlend);
    }
}
