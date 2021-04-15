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
    [SerializeField] private Vector2Variable _cursorScreenPositionAtoms;
    [SerializeField] private Vector3Variable _smoothLocalInputMovementAtoms;
    [Space(10)]
    [Header("Movement Settings")]
    [SerializeField] private FloatVariable _movementSpeedAtoms;
    [SerializeField] private FloatVariable _dashSpeedAtoms;
    [Space(10)]
    [Range(1, 10)] public float MovementSmoothSpeedStart = 2.0f;
    [Range(1, 10)] public float MovementSmoothSpeedEnd = 2.0f;
    [Space(10)]
    [Min(0)] public float TurnSpeed = 1.0f;
    [Header("Aim Settings")]
    [Tooltip("Only for controllers")] public float DeltaCursorSpeed = 3.0f;
    public LayerMask AimLayerMask;
    #endregion

    private Camera _mainCamera;
    private Rigidbody _body;

    private Plane _plane = new Plane(Vector3.up, 0);

    private Coroutine _coroutineUpdateAimOverTime;

    private PlayerControls _input;

    private Vector3 _smoothInputMovement;
    private Vector3 _cursorWorldPosition;
    private Vector3 _smoothLocalInputMovement;
    private Vector3 _rawInputMovement;
    private Vector3 _cursorWorldDirection = Vector3.forward;
    private Vector2 _cursorScreenPosition;
    private Vector2 _deltaCursor;
    private float _movementSpeed;
    private float _dashSpeed;
    private bool _isDeltaCursor;
    private bool _isMoving;
    private bool _isDashing;
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
    public void OnDashSpeedChange(float speed) => _dashSpeed = speed;

    public void OnCursorPosition(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = context.ReadValue<Vector2>();
        if (screenPosition != Vector2.zero)
            _cursorScreenPosition = context.ReadValue<Vector2>();

        _cursorScreenPositionAtoms.Value = _cursorScreenPosition;

        UpdateCursor();
    }

    public void OnCursorDelta(InputAction.CallbackContext context)
    {
        _deltaCursor = context.ReadValue<Vector2>();
        Debug.Log(_deltaCursor + " " + context.started + " " + context.canceled);
        _isDeltaCursor = (_deltaCursor != Vector2.zero);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputMovement = context.ReadValue<Vector2>();
        _rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);

        _isMoving = (inputMovement != Vector2.zero);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        _isDashing = (context.ReadValue<float>() > 0.5f);
        _body.AddForce(transform.forward * _dashSpeed * _body.mass, ForceMode.Impulse);
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
    }

    private void OnEnable()
    {
        _coroutineUpdateAimOverTime = StartCoroutine(UpdateCursorOverTime());

        _input.Enable();
        _input.Ground.Move.performed += OnMove;
        _input.Ground.Move.canceled += OnMove;
        _input.Ground.Camera.performed += OnCamera;
        _input.Ground.Camera.canceled += OnCamera;
        _input.Ground.Dash.performed += OnDash;
        _input.Ground.Dash.canceled += OnDash;
        _input.Ground.CursorPosition.performed += OnCursorPosition;
        _input.Ground.CursorPosition.canceled += OnCursorPosition;
        _input.Ground.CursorDelta.performed += OnCursorDelta;
        _input.Ground.CursorDelta.canceled += OnCursorDelta;
        _movementSpeedAtoms.Changed.Register(OnMovementSpeedChange);
        _dashSpeedAtoms.Changed.Register(OnDashSpeedChange);
    }

    private void Update()
    {
        UpdateSmoothInputMovement();
        UpdateDeltaCursor();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateTurn();
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutineUpdateAimOverTime);

        _input.Disable();
        _input.Ground.Move.performed -= OnMove;
        _input.Ground.Move.canceled -= OnMove;
        _input.Ground.Camera.performed -= OnCamera;
        _input.Ground.Camera.canceled -= OnCamera;
        _input.Ground.Dash.performed -= OnDash;
        _input.Ground.Dash.canceled -= OnDash;
        _input.Ground.CursorPosition.performed -= OnCursorPosition;
        _input.Ground.CursorPosition.canceled -= OnCursorPosition;
        _input.Ground.CursorDelta.performed -= OnCursorDelta;
        _input.Ground.CursorDelta.canceled -= OnCursorDelta;
        _movementSpeedAtoms.Changed.Unregister(OnMovementSpeedChange);
        _dashSpeedAtoms.Changed.Unregister(OnDashSpeedChange);
    }
    #endregion

    private void UpdateSmoothInputMovement()
    {
        _smoothInputMovement = Vector3.Lerp(_smoothInputMovement, _rawInputMovement,
                Time.deltaTime * (_isMoving ? MovementSmoothSpeedStart : MovementSmoothSpeedEnd));

        // Transform smooth input movement to the game object's local space.
        _smoothLocalInputMovement = transform.InverseTransformDirection(_mainCamera.transform.TransformDirection(_smoothInputMovement));

        _smoothLocalInputMovementAtoms.Value = _smoothLocalInputMovement;
    }

    private void UpdateMovement()
    {
        Vector3 velocity = GetCameraDirection * _movementSpeed * Time.deltaTime;
        _body.MovePosition(transform.position + velocity);
    }

    private void UpdateTurn()
    {
        Quaternion rotation = Quaternion.Slerp(_body.rotation, Quaternion.LookRotation(_cursorWorldDirection), TurnSpeed * Time.deltaTime);
        _body.MoveRotation(rotation);
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
            _cursorScreenPosition += _deltaCursor * DeltaCursorSpeed;
            _cursorScreenPositionAtoms.Value = _cursorScreenPosition;

            UpdateCursor();
        }
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
}
