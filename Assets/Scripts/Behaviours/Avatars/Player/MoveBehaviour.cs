using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using MyBox;

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
    public bool IsMoving
    {
        get => _isMoving;
        set => _isMoving = value;
    }

    public bool IsRotating
    {
        get => _isRotating;
        set => _isRotating = value;
    }

    public Vector3 Velocity
    {
        get => CameraDirection * _movementSpeed * Time.fixedDeltaTime;
    }

    public Vector3 SmoothInputMovement
    {
        get => _smoothInputMovement;
        set => _smoothInputMovement = value;
    }

    public Vector3 PointDirection
    {
        get
        {
            Vector3 playerPosition = transform.position;
            Vector3 _pointWorldPosition = _pointWorldPositionVariable.Value;

            return (_pointWorldPosition != Vector3.zero) ? new Vector3(_pointWorldPosition.x - playerPosition.x, 0, _pointWorldPosition.z - playerPosition.z) : Vector3.forward;
        }
    }

    public Vector3 CameraDirection
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
    [Space(10)]
    [Header("Movement Settings")]
    [SerializeField] private FloatVariable _movementSpeedAtoms;
    [SerializeField] private float _speedForwardMultiplier = 1.0f;
    [SerializeField] private float _speedBackwardMultiplier = 0.5f;
    [SerializeField] private float _speedSideMultiplier = 0.6f;
    [SerializeField, Range(1, 10)] private float _movementSmoothSpeedStart = 2.0f;
    [SerializeField, Range(1, 10)] private float _movementSmoothSpeedEnd = 2.0f;
    [SerializeField, PositiveValueOnly] private float _turnSpeed = 3.0f;
    [Space(10)]
    [Header("Fall Setting")]
    [SerializeField] private LayerMask _fallLayerMask;
    [SerializeField] private LandData[] _landData;
    [Foldout("Atoms", true)]
    [SerializeField] private Vector3Variable _pointWorldPositionVariable;
    [SerializeField] private Vector3Variable _smoothLocalInputMovementVariable;
    [SerializeField] private Vector3Variable _rawLocalInputMovementVariable;
    [SerializeField] private BoolVariable _fallingVariable;
    [SerializeField] private ColliderEvent _groundCheckEventEnter;
    [SerializeField] private ColliderEvent _groundCheckEventExit;
    [SerializeField] private AnimatorModifierEvent _animatorModifierEvent;
    [SerializeField] private VoidEvent _movementEnterEvent;
    [SerializeField] private VoidEvent _movementExitEvent;
    [SerializeField] private DurationValueList _stunMoveList;
    #endregion

    [Serializable]
    private struct LandData
    {
        [MinMaxRange(0, 4)] public MinMaxFloat Timer;
        public AnimationClip Animation;
        public float AnimationSpeedMultiplier;
        public float StunMultiplier;
    }

    private Camera _mainCamera;
    private Rigidbody _rigidbody;

    private PlayerControls _controls;

    private MovementType _movementType;

    private Stopwatch _landStopWatch;

    private Coroutine _coroutineStunned;

    private Vector3 _smoothInputMovement;
    private Vector3 _smoothLocalInputMovement;
    private Vector3 _rawInputMovement;
    private Vector3 _smoothVelocity;
    private float _movementSpeed;
    private int _groundCheckEntered;
    private bool _isMoving = true;
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
        _landStopWatch = new Stopwatch(0);
    }

    public void OnLand()
    {
        foreach (var data in _landData)
        {
            if (_landStopWatch.Timer >= data.Timer.Min && _landStopWatch.Timer <= data.Timer.Max)
            {
                AnimatorModifier modifier = new AnimatorModifier(data.Animation, data.AnimationSpeedMultiplier);
                _animatorModifierEvent.Raise(modifier);

                _stunMoveList.Add(new Duration(data.Animation.length / data.AnimationSpeedMultiplier * data.StunMultiplier));

                _smoothInputMovement = Vector3.zero;

                break;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputMovement = context.ReadValue<Vector2>();

        _rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);

        if (context.started)
        {
            _smoothVelocity = Velocity;

            _movementEnterEvent.Raise();
        }
        else if (context.canceled)
        {
            _smoothVelocity = Velocity / 2;

            _movementExitEvent.Raise();
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
        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _groundCheckEntered = 0;
        _controls.Ground.Move.started += OnMove;
        _controls.Ground.Move.performed += OnMove;
        _controls.Ground.Move.canceled += OnMove;
        _movementSpeedAtoms.Changed.Register(OnMovementSpeedChange);
        _groundCheckEventEnter.Register(OnEnterGroundCheck);
        _groundCheckEventExit.Register(OnExitGroundCheck);
        _controls.Enable();

        _coroutineStunned = StartCoroutine(UpdateStunned());
    }

    private void Update()
    {
        UpdateCameraPivot();
        UpdateRawInputMovement();
        UpdateSmoothInputMovement();
    }

    private void FixedUpdate()
    {
        if (_isMoving)
            FixedUpdateMovement();

        if (_isRotating)
            FixedUpdateTurn();
    }

    private void OnDisable()
    {
        _controls.Ground.Move.started -= OnMove;
        _controls.Ground.Move.performed -= OnMove;
        _controls.Ground.Move.canceled -= OnMove;
        _movementSpeedAtoms.Changed.Unregister(OnMovementSpeedChange);
        _groundCheckEventEnter.Unregister(OnEnterGroundCheck);
        _groundCheckEventExit.Unregister(OnExitGroundCheck);
        _controls.Disable();

        StopCoroutine(_coroutineStunned);
    }
    #endregion

    private void UpdateCameraPivot()
    {
        // Transform camera pivot to look away from the camera.
        Vector3 cameraPosition = _mainCamera.transform.position;
        Vector3 cameraPivotPosition = _cameraPivotTransform.transform.position;
        _cameraPivotTransform.rotation =
            Quaternion.LookRotation(cameraPivotPosition - new Vector3(cameraPosition.x, cameraPivotPosition.y, cameraPosition.z));

    }

    private void UpdateRawInputMovement()
    {
        // Transform raw input movement to the game object's local space.
        _rawLocalInputMovementVariable.Value =
            transform.InverseTransformDirection(_cameraPivotTransform.TransformDirection(_rawInputMovement));
    }

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
                Time.deltaTime * (_rawInputMovement != Vector3.zero ? _movementSmoothSpeedStart : _movementSmoothSpeedEnd));

        // Transform smooth input movement to the game object's local space.
        _smoothLocalInputMovementVariable.Value = _smoothLocalInputMovement =
            transform.InverseTransformDirection(_cameraPivotTransform.TransformDirection(_smoothInputMovement));

        // Get movement type based on the angle of directional movement.
        float angle = (_rawInputMovement != Vector3.zero) ? Vector3.Angle(Vector3.forward, _smoothLocalInputMovement.normalized) : -1.0f;

        _movementType = GetMovementType(angle);
    }

    private void FixedUpdateMovement()
    {
        Vector3 localVelocity = Velocity;

        switch (_movementType)
        {
            case MovementType.Forward:
                localVelocity *= _speedForwardMultiplier;
                break;
            case MovementType.Forward | MovementType.Side:
                localVelocity *= (_speedForwardMultiplier + _speedSideMultiplier) / 2;
                break;
            case MovementType.Side:
                localVelocity *= _speedSideMultiplier;
                break;
            case MovementType.Backward | MovementType.Side:
                localVelocity *= (_speedBackwardMultiplier + _speedSideMultiplier) / 2;
                break;
            case MovementType.Backward:
                localVelocity *= _speedBackwardMultiplier;
                break;
        }

        _smoothVelocity = Vector3.Lerp(_smoothVelocity, localVelocity, _movementSpeed * Time.fixedDeltaTime);

        _rigidbody.MovePosition(transform.position + _smoothVelocity);
    }

    private void FixedUpdateTurn()
    {
        Quaternion rotation = Quaternion.Slerp(_rigidbody.rotation, Quaternion.LookRotation(PointDirection), _turnSpeed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(rotation);
    }

    private IEnumerator UpdateStunned()
    {
        yield return new WaitForFixedUpdate();

        while (true)
        {
            for (int i = 0; i < _stunMoveList.Count; i++)
            {
                Duration duration = _stunMoveList[i];

                if (!duration.IsActive)
                    _stunMoveList.Remove(duration);
            }

            if (_stunMoveList.Count != 0)
            {
                _isRotating = false;
                _controls.Disable();
            }
            else
            {
                _isRotating = true;
                _controls.Enable();
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
