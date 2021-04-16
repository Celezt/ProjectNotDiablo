using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class PlayerAnimation : MonoBehaviour
{
    #region Inspector
    [Header("Atoms")]
    [SerializeField] private Vector3Variable _smoothLocalInputMovementAtoms;
    [SerializeField] private MovementTypeVariable _movementTypeAtoms;
    [Space(10)]
    [Header("Animations Settings")]
    [SerializeField] private Animator _animation;
    [Space(10)]
    public float IdleBlendSpeed = 0.002f;
    public float MotionBlendMargin = 0.01f;
    #endregion

    private MovementType _movementType;

    private float _idleBlend;

    private readonly int _motionZID = Animator.StringToHash("MotionZ");
    private readonly int _motionXID = Animator.StringToHash("MotionX");
    private readonly int _idleID = Animator.StringToHash("IdleBlend");

    #region Events
    public void OnMovementTypeChange(MovementType value) => _movementType = value;
    #endregion

    #region Unity Message
    private void OnEnable()
    {
        _movementTypeAtoms.Changed.Register(OnMovementTypeChange);
    }

    private void Update()
    {
        UpdateMotionAnimation();
        UpdateIdleAnimation();
    }

    private void OnDisable()
    {
        _movementTypeAtoms.Changed.Unregister(OnMovementTypeChange);
    }
    #endregion

    private void UpdateMotionAnimation()
    {
        if (_smoothLocalInputMovementAtoms != null)
        {
            Vector3 blend = _smoothLocalInputMovementAtoms.Value;
            _animation.SetFloat(_motionZID, blend.z);
            _animation.SetFloat(_motionXID, blend.x);
        }
    }

    private void UpdateIdleAnimation()
    {
        _idleBlend = (_idleBlend + IdleBlendSpeed) % 1;
        _animation.SetFloat(_idleID, _idleBlend);
    }
}
