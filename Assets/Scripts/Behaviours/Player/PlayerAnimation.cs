using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class PlayerAnimation : MonoBehaviour
{
    #region Inspector
    [Header("Atoms")]
    [SerializeField] private Vector3Variable _smoothLocalInputMovementAtoms;
    [Space(10)]
    [Header("Animations Settings")]
    [SerializeField] private Animator _animation;
    [Space(10)]
    public float IdleBlendSpeed = 0.002f;
    public float MotionBlendMargin = 0.01f;
    #endregion

    private float _idleBlend;

    private readonly int _motionID = Animator.StringToHash("Motion");
    private readonly int _idleID = Animator.StringToHash("Idle");
    private readonly int _dashID = Animator.StringToHash("IsDashing");

    #region Events
    public void OnDashChange(bool value) => _animation.SetBool(_dashID , value);
    #endregion

    #region Unity Message
    private void Update()
    {
        UpdateMotionAnimation();
        UpdateIdleAnimation();
    }
    #endregion

    private void UpdateMotionAnimation()
    {
        if (_smoothLocalInputMovementAtoms != null)
        {
            float blend = _smoothLocalInputMovementAtoms.Value.magnitude;
            _animation.SetFloat(_motionID, (blend > 0.01f) ? blend : 0.0f);
        }
    }

    private void UpdateIdleAnimation()
    {
        _idleBlend = (_idleBlend + IdleBlendSpeed) % 1;
        _animation.SetFloat(_idleID, _idleBlend);
    }
}
