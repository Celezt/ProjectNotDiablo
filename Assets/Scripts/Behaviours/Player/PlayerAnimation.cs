using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class PlayerAnimation : MonoBehaviour
{
    #region Inspector
    [Header("Setup")]
    [SerializeField] private PlayerController _controller;
    [SerializeField] private Animator _animation;
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
            Animator.StringToHash("Motion"),
            Animator.StringToHash("Idle"),
            Animator.StringToHash("IsDashing")
        );
    }

    private void Update()
    {
        UpdateMotionAnimation();
        UpdateIdleAnimation();
        UpdateDashAnimation();
    }
    #endregion

    private void UpdateDashAnimation()
    {
        if (_controller != null)
            _animation.SetBool(_id.Dash, _controller.IsDashing);
    }

    private void UpdateMotionAnimation()
    {
        if (_controller != null)
        {
            float blend = _controller.SmoothInputMovement.magnitude;
            _animation.SetFloat(_id.Motion, (blend > 0.01f) ? blend : 0.0f);
        }
    }

    private void UpdateIdleAnimation()
    {
        _idleBlend = (_idleBlend + IdleBlendSpeed) % 1;
        _animation.SetFloat(_id.Idle, _idleBlend);
    }
}
