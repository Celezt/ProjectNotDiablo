using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Inspector
    [Header("Atoms")]
    [SerializeField] private Vector2Variable _cursorScreenPositionVariable;
    [SerializeField] private Vector3Variable _smoothLocalInputMovementVariable;
    [SerializeField] private MovementTypeVariable _movementTypeVariable;
    [SerializeField] private CooldownValueList _invisibleFrameVariable;
    [SerializeField] private AnimatorModifierEvent _animatoinModifierEvent;
    [SerializeField] private AnimatorModifierInfoEvent _animatoinModifierInfoEvent;
    [Space(10)]
    [Header("Movement Settings")]
    [SerializeField] private FloatVariable _movementSpeedAtoms;
    [Space(10)]
    public float SpeedForwardMultiplier = 1.0f;
    public float SpeedBackwardMultiplier = 0.5f;
    public float SpeedSideMultiplier = 0.6f;
    [Space(10)]
    [Range(1, 10)] public float MovementSmoothSpeedStart = 2.0f;
    [Range(1, 10)] public float MovementSmoothSpeedEnd = 2.0f;
    [Space(10)]
    [Min(0)] public float TurnSpeed = 3.0f;
    [Space(10)]
    [Header("Roll Settings")]
    public float RollInvisibilityLength = 1.0f;
    [SerializeField] private AnimationClip _rollAnimation;
    public float RollAnimationSpeedMultiplier = 1.0f;
    [Space(10)]
    [Header("Aim Settings")]
    [Tooltip("Only for controllers")] public float DeltaCursorSpeed = 600.0f;
    public LayerMask AimLayerMask;
    #endregion

    private Camera _mainCamera;
    private Rigidbody _body;

    private Plane _plane = new Plane(Vector3.up, 0);
    private Transform _cameraPivotTransform;

    private Coroutine _coroutineUpdateAimOverTime;

    private PlayerControls _input;

    private MovementType _movementType;

    private Vector3 _smoothInputMovement;
    private Vector3 _cursorWorldPosition;
    private Vector3 _smoothLocalInputMovement;
    private Vector3 _rawInputMovement;
    private Vector3 _cursorWorldDirection = Vector3.forward;
    private Vector2 _cursorScreenPosition;
    private Vector2 _deltaCursor;
    private float _movementSpeed;
    private bool _isDeltaCursor;
    private bool _isMoving;
    private bool _isRolling;
    private bool _isCameraAngleChanged;

    private Vector3 GetCameraDirection
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

    private Vector3 GetAimDirection
    {
        get
        {
            Vector3 playerPosition = transform.position;

            return (_cursorWorldPosition != Vector3.zero) ? new Vector3(_cursorWorldPosition.x - playerPosition.x, 0, _cursorWorldPosition.z - playerPosition.z) : Vector3.forward;
        }
    }

    #region Events
    public void OnMovementSpeedChange(float speed) => _movementSpeed = speed;

    public void OnCursorPosition(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = context.ReadValue<Vector2>();
        if (screenPosition != Vector2.zero)
            _cursorScreenPosition = context.ReadValue<Vector2>();

        _cursorScreenPositionVariable.Value = _cursorScreenPosition;

        UpdateCursor();
    }

    public void OnCursorDelta(InputAction.CallbackContext context)
    {
        _deltaCursor = context.ReadValue<Vector2>();
        _isDeltaCursor = (_deltaCursor != Vector2.zero);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputMovement = context.ReadValue<Vector2>();
        _rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);

        _isMoving = (inputMovement != Vector2.zero);
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        _isRolling = (context.ReadValue<float>() > 0.5f);

        if (context.performed)
        {
            StartCoroutine(WhileRollAction());

            AnimatorModifier modifier = new AnimatorModifier(_rollAnimation, RollAnimationSpeedMultiplier);
            modifier.Clip = _rollAnimation;
            _animatoinModifierEvent.Raise(modifier);
        }
    }

    public void OnExitRoll(AnimatorModifierInfo animatorInfo)
    {

    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        _isCameraAngleChanged = (context.ReadValue<Vector2>() != Vector2.zero);
    }
    #endregion

    #region Unity Message
    private void Awake()
    {
        _input = new PlayerControls();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _body = GetComponent<Rigidbody>();
        _cameraPivotTransform = transform.Find("CameraPivot");
    }

    private void OnEnable()
    {
        _coroutineUpdateAimOverTime = StartCoroutine(UpdateCursorOverTime());

        _input.Enable();
        _input.Ground.Move.performed += OnMove;
        _input.Ground.Move.canceled += OnMove;
        _input.Ground.Camera.performed += OnCamera;
        _input.Ground.Camera.canceled += OnCamera;
        _input.Ground.Roll.performed += OnRoll;
        _input.Ground.Roll.canceled += OnRoll;
        _input.Ground.CursorPosition.performed += OnCursorPosition;
        _input.Ground.CursorPosition.canceled += OnCursorPosition;
        _input.Ground.CursorDelta.performed += OnCursorDelta;
        _input.Ground.CursorDelta.canceled += OnCursorDelta;
        _animatoinModifierInfoEvent.Register(OnExitRoll);
        _movementSpeedAtoms.Changed.Register(OnMovementSpeedChange);
    }

    private void Update()
    {
        UpdateDeltaCursor();
        UpdateSmoothInputMovement();
    }

    private void FixedUpdate()
    {
        FixedUpdateMovement();
        FixedUpdateTurn();
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutineUpdateAimOverTime);

        _input.Disable();
        _input.Ground.Move.performed -= OnMove;
        _input.Ground.Move.canceled -= OnMove;
        _input.Ground.Camera.performed -= OnCamera;
        _input.Ground.Camera.canceled -= OnCamera;
        _input.Ground.Roll.performed -= OnRoll;
        _input.Ground.Roll.canceled -= OnRoll;
        _input.Ground.CursorPosition.performed -= OnCursorPosition;
        _input.Ground.CursorPosition.canceled -= OnCursorPosition;
        _input.Ground.CursorDelta.performed -= OnCursorDelta;
        _input.Ground.CursorDelta.canceled -= OnCursorDelta;
        _animatoinModifierInfoEvent.Unregister(OnExitRoll);
        _movementSpeedAtoms.Changed.Unregister(OnMovementSpeedChange);
    }
    #endregion

    private void UpdateSmoothInputMovement()
    {
        static MovementType GetMovementType(float angle) => angle switch
        {
            var v when v >= 150.0f => MovementType.Backward,
            var v when v > 100.0f => MovementType.Backward | MovementType.Side,
            var v when v > 80.0f => MovementType.Side,
            var v when v > 50.0f => MovementType.Forward | MovementType.Side,
            var v when v >= 0.0f => MovementType.Forward,
            _ => MovementType.None,
        };

        _smoothInputMovement = Vector3.Lerp(_smoothInputMovement, _rawInputMovement,
                Time.deltaTime * (_isMoving ? MovementSmoothSpeedStart : MovementSmoothSpeedEnd));

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
        _movementTypeVariable.Value = _movementType = GetMovementType(angle);
    }

    private void UpdateCursor()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_cursorScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, AimLayerMask))
            _cursorWorldPosition = hit.point;
        else if (_plane.Raycast(ray, out float distance))   // Collide with a plane if no object was collided with.
            _cursorWorldPosition = ray.GetPoint(distance);

        _cursorWorldDirection = GetAimDirection;
    }

    // Update aim if aiming using a controller.
    private void UpdateDeltaCursor()
    {
        if (_isDeltaCursor)
        {
            _cursorScreenPosition += _deltaCursor * Time.deltaTime * DeltaCursorSpeed;
            _cursorScreenPositionVariable.Value = _cursorScreenPosition;

            UpdateCursor();
        }
    }

    private void FixedUpdateMovement()
    {
        Vector3 velocity = GetCameraDirection * _movementSpeed * Time.deltaTime;

        switch (_movementType)
        {
            case MovementType.Forward:
                velocity *= SpeedForwardMultiplier;
                break;
            case MovementType.Forward | MovementType.Side:
                velocity *= (SpeedForwardMultiplier + SpeedSideMultiplier) / 2;
                break;
            case MovementType.Side:
                velocity *= SpeedSideMultiplier;
                break;
            case MovementType.Backward | MovementType.Side:
                velocity *= (SpeedBackwardMultiplier + SpeedSideMultiplier) / 2;
                break;
            case MovementType.Backward:
                velocity *= SpeedBackwardMultiplier;
                break;
        }

        _body.MovePosition(transform.position + velocity);
    }

    private void FixedUpdateTurn()
    {
        Quaternion rotation = Quaternion.Slerp(_body.rotation, Quaternion.LookRotation(_cursorWorldDirection), TurnSpeed * Time.deltaTime);
        _body.MoveRotation(rotation);
    }

    // To prevent loosing aim when not performing any aim movement.
    private IEnumerator UpdateCursorOverTime()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            yield return new WaitUntil(() => !_isCameraAngleChanged);    // Only update over time if not changing the camera
            UpdateCursor();
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator WhileRollAction()
    {
        Cooldown invisibility = new Cooldown(RollInvisibilityLength);
        _invisibleFrameVariable.Add(invisibility);
        yield return new WaitForSeconds(RollInvisibilityLength);
        _invisibleFrameVariable.Remove(invisibility);
    }
}
