using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private PlayerController _controller;
    [SerializeField] private Animator _animation;

    [Header("Settings")]
    public float IdleBlendSpeed = 0.002f;
    public float MotionBlendMargin = 0.01f;

    private ParameterID _id;

    private float _idleBlend;

    private readonly struct ParameterID
    {
        public readonly int Motion;
        public readonly int Idle;

        public ParameterID(int motion, int idle)
        {
            Motion = motion;
            Idle = idle;
        }
    }

    #region Unity Message
    private void Start()
    {
        _id = new ParameterID(
            Animator.StringToHash("Velocity"),
            Animator.StringToHash("Idle")
        );
    }

    private void Update()
    {
        UpdateMotionAnimation();
        UpdateIdleAnimation();
    }
    #endregion

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
