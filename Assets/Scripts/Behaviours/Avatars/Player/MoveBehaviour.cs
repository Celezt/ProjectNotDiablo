using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;

[RequireComponent(typeof(Rigidbody))]
public class MoveBehaviour : MonoBehaviour
{
    public PlayerControls Controls
    {
        get => _controls;
    }

    public bool IsFalling
    {
        get => _groundCheckEntered == 0;
    }

    public bool IsRotating
    {
        get => _isRotating;
        set => _isRotating = value;
    }

    public Vector3 GetCameraDirection
    {
        get
        {
            Transform cameraTransform = _mainCamera.transform;
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            return cameraForward * _smoothInputMovement.z + cameraRight * _smoothInputMovement.x;
        }
    }

    #region Inspector
    [SerializeField] private Transform _cameraPivotTransform;
    [SerializeField] private DodgeBehaviour _dodgeBehaviour;
    [Space(10)]
    [Header("Atoms")]
    [SerializeField] private Vector3Variable _smoothLocalInputMovementVariable;
    [SerializeField] private Vector3Variable _pointWorldDirectionVariable;
    [SerializeField] private BoolVariable _fallingVariable;
    [SerializeField] private ColliderEvent _groundCheckEventEnter;
    [SerializeField] private ColliderEvent _groundCheckEventExit;
    [SerializeField] private AnimatorModifierEvent _animatorModifierEvent;
    [SerializeField] private AnimatorModifierInfoEvent _animatorModifierInfoEvent;
    [Space(10)]
    [Header("Movement Settings")]
    [SerializeField] private FloatVariable _movementSpeedAtoms;
    [Space(10)]
    [SerializeField] private float _speedForwardMultiplier = 1.0f;
    [SerializeField] private float _speedBackwardMultiplier = 0.5f;
    [SerializeField] private float _speedSideMultiplier = 0.6f;
    [Space(10)]
    [SerializeField, Range(1, 10)] private float _movementSmoothSpeedStart = 2.0f;
    [SerializeField, Range(1, 10)] private float _movementSmoothSpeedEnd = 2.0f;
    [Space(10)]
    [SerializeField, Min(0)] private float _turnSpeed = 3.0f;
    [Space(10)]
    [Header("Fall Setting")]
    [SerializeField] private LayerMask _fallLayerMask;
    [SerializeField] private float _landAnimationSpeedMultiplier = 2.0f;
    [SerializeField] private float _heavyLandAnimationSpeedMultiplier = 1.0f;
    [SerializeField] private float _heavyLandTime = 1.0f;
    [SerializeField] private AnimationClip _landAnimation;
    #endregion

    private Camera _mainCamera;
    private Rigidbody _rigidbody;

    private PlayerControls _controls;

    private MovementType _movementType;

    private Duration _heavyLandTimer;

    private Vector3 _smoothInputMovement;
    private Vector3 _smoothLocalInputMovement;
    private Vector3 _rawInputMovement;
    private Vector3 _smoothVelocity;
    private float _movementSpeed;
    private int _groundCheckEntered;
    private bool _isMoving;
    private bool _isRotating = true;

    [Flags]
    private enum MovementType
    {
        None = 0b_0000_0000,
        Forward = 0b_0000_0001,
        Backward = 0b_0000_0010,
        Side = 0b_0000_0100,
    }

    #region Events
    public void OnMovementSpeedChange(float speed) => _movementSpeed = speed;

    public void OnEnterGroundCheck(Collider collider)
    {
        if (IsFalling)
        {
            OnLand();
            _fallingVariable.Value = false;
        }

        _groundCheckEntered++;
    }

    public void OnExitGroundCheck(Collider collider)
    {
        _groundCheckEntered--;
        _fallingVariable.Value = IsFalling;
        _heavyLandTimer = new Duration(_heavyLandTime);
    }

    public void OnLand()
    {
        float multiplier = 0;

        // If it should use heavy or light land animation speed.
        if (_heavyLandTimer.IsActive)
            multiplier = _landAnimationSpeedMultiplier;
        else
            multiplier = _heavyLandAnimationSpeedMultiplier;

        AnimatorModifier modifier = new AnimatorModifier(_landAnimation, multiplier);
        _animatorModifierEvent.Raise(modifier);

        _controls.Disable();
        _dodgeBehaviour?.Controls.Disable();

        _smoothInputMovement = Vector3.zero;
    }

    public void OnExitLand(AnimatorModifierInfo animatorInfo)
    {
        _controls.Enable();
        _dodgeBehaviour?.Controls.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputMovement = context.ReadValue<Vector2>();

        _rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
        _isMoving = (inputMovement != Vector2.zero);
    }
    #endregion

    #region Unity Message
    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _controls.Ground.Move.performed += OnMove;
        _controls.Ground.Move.canceled += OnMove;
        _movementSpeedAtoms.Changed.Register(OnMovementSpeedChange);
        _groundCheckEventEnter.Register(OnEnterGroundCheck);
        _groundCheckEventExit.Register(OnExitGroundCheck);
        _animatorModifierInfoEvent.Register(OnExitLand);
        _controls.Enable();
    }

    private void Update()
    {
        UpdateSmoothInputMovement();
    }

    private void FixedUpdate()
    {
        FixedUpdateMovement();

        if (_isRotating)
            FixedUpdateTurn();
    }

    private void OnDisable()
    {
        _controls.Ground.Move.performed -= OnMove;
        _controls.Ground.Move.canceled -= OnMove;
        _movementSpeedAtoms.Changed.Unregister(OnMovementSpeedChange);
        _groundCheckEventEnter.Unregister(OnEnterGroundCheck);
        _groundCheckEventExit.Unregister(OnExitGroundCheck);
        _animatorModifierInfoEvent.Unregister(OnExitLand);
        _controls.Disable();
    }
    #endregion

    private void UpdateSmoothInputMovement()
    {
        static MovementType GetMovementType(float angle) => angle switch
        {
            var v when v >= 150.0f => MovementType.Backward,
            var v when v > 100.0f => MovementType.Backward | MovementType.Side,
            var v when v > 80.0f => MovementType.Side,
            var v when v > 30.0f => MovementType.Forward | MovementType.Side,
            var v when v >= 0.0f => MovementType.Forward,
            _ => MovementType.None,
        };

        _smoothInputMovement = Vector3.Lerp(_smoothInputMovement, _rawInputMovement,
                Time.deltaTime * (_isMoving ? _movementSmoothSpeedStart : _movementSmoothSpeedEnd));

        // Transform camera pivot to look away from the camera.
        Vector3 cameraPosition = _mainCamera.transform.position;
        Vector3 cameraPivotPosition = _cameraPivotTransform.transform.position;
        _cameraPivotTransform.rotation =
            Quaternion.LookRotation(cameraPivotPosition - new Vector3(cameraPosition.x, cameraPivotPosition.y, cameraPosition.z));

        // Transform smooth input movement to the game object's local space.
        _smoothLocalInputMovementVariable.Value = _smoothLocalInputMovement =
            transform.InverseTransformDirection(_cameraPivotTransform.TransformDirection(_smoothInputMovement));

        // Get movement type based on the angle of directional movement.
        float angle = (_rawInputMovement != Vector3.zero) ? Vector3.Angle(Vector3.forward, _smoothLocalInputMovement.normalized) : -1.0f;

        _movementType = GetMovementType(angle);
    }

    private void FixedUpdateMovement()
    {
        Vector3 velocity = GetCameraDirection * _movementSpeed * Time.fixedDeltaTime;

        switch (_movementType)
        {
            case MovementType.Forward:
                velocity *= _speedForwardMultiplier;
                break;
            case MovementType.Forward | MovementType.Side:
                velocity *= (_speedForwardMultiplier + _speedSideMultiplier) / 2;
                break;
            case MovementType.Side:
                velocity *= _speedSideMultiplier;
                break;
            case MovementType.Backward | MovementType.Side:
                velocity *= (_speedBackwardMultiplier + _speedSideMultiplier) / 2;
                break;
            case MovementType.Backward:
                velocity *= _speedBackwardMultiplier;
                break;
        }

        _smoothVelocity = Vector3.Lerp(_smoothVelocity, velocity, _movementSpeed * Time.fixedDeltaTime);

        _rigidbody.MovePosition(transform.position + _smoothVelocity);
    }

    private void FixedUpdateTurn()
    {
        Quaternion rotation = Quaternion.Slerp(_rigidbody.rotation, Quaternion.LookRotation(_pointWorldDirectionVariable.Value), _turnSpeed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(rotation);
    }
}
