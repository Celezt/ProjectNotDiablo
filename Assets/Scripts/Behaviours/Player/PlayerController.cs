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
    [Header("Movement Settings")]
    [SerializeField] private FloatVariable _movementSpeedAtoms;
    [SerializeField] private FloatVariable _dashSpeedAtoms;
    [Space(10)]
    [Range(1, 10)] public float MovementSmoothSpeedStart = 2.0f;
    [Range(1, 10)] public float MovementSmoothSpeedEnd = 2.0f;
    [Space(10)]
    [Min(0)] public float TurnSpeed = 1.0f;
    [Header("Aim Settings")]
    [Tooltip("Only for controllers")] public float DeltaAimSpeed = 3.0f;
    public LayerMask AimLayerMask;
    #endregion

    public bool IsMoving { get; set; }
    public bool IsDashing { get; set; }
    public bool IsCameraAngleChanged { get; set; }
    public Vector3 RawInputMovement { get; set; }
    public Vector3 SmoothInputMovement { get; set; }
    public Vector3 LocalVelocity { get; set; }
    public Vector2 AimScreenPosition { get; set; }
    public Vector3 AimWorldPosition { get; set; }

    private Camera _mainCamera;
    private Rigidbody _body;

    private Coroutine _coroutineUpdateAimOverTime;
    private Coroutine _coroutineDeltaAim;

    private Vector3 _oldPosition;
    private float _movementSpeed;
    private float _dashSpeed;
    private Vector3 _aimWorldDirection = Vector3.forward;
    private Vector2 _deltaAim;
    private bool _isDeltaAim;

    private Vector3 GetCameraDirection
    {
        get
        {
            Transform cameraTransform = _mainCamera.transform;
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            return cameraForward * SmoothInputMovement.z + cameraRight * SmoothInputMovement.x;
        }
    }

    private Vector3 GetAimDirection
    {
        get
        {
            Vector3 playerPosition = transform.position;

            return (AimWorldPosition != Vector3.zero) ? new Vector3(AimWorldPosition.x - playerPosition.x, 0, AimWorldPosition.z - playerPosition.z) : Vector3.forward;
        }
    }

    #region Events
    public void OnMovementChange(float speed) => _movementSpeed = speed;
    public void OnDashChange(float speed) => _dashSpeed = speed;

    public void OnAim(InputAction.CallbackContext value)
    {
        Vector2 screenPosition = value.ReadValue<Vector2>();
        if (screenPosition != Vector2.zero)
            AimScreenPosition = value.ReadValue<Vector2>();

        UpdateAim();
    }

    public void OnDeltaAim(InputAction.CallbackContext value)
    {
        _deltaAim = value.ReadValue<Vector2>();

        _isDeltaAim = (_deltaAim != Vector2.zero);
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        RawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);

        IsMoving = (inputMovement != Vector2.zero);
    }

    public void OnDash(InputAction.CallbackContext value)
    {
        IsDashing = (value.ReadValue<float>() > 0.5f);
        _body.AddForce(transform.forward * _dashSpeed * _body.mass, ForceMode.Impulse);
    }

    public void OnCamera(InputAction.CallbackContext value)
    {
        IsCameraAngleChanged = (value.ReadValue<Vector2>() != Vector2.zero);
    }

    public void OnDeviceLost(PlayerInput input)
    {

    }

    public void OnDeviceRegained(PlayerInput input)
    {

    }

    public void OnControlsChanged(PlayerInput input)
    {

    }
    #endregion

    #region Unity Message

    private void Start()
    {
        _mainCamera = Camera.main;
        _body = GetComponent<Rigidbody>();

        _oldPosition = transform.position;
    }

    private void OnEnable()
    {
        _coroutineUpdateAimOverTime = StartCoroutine(UpdateAimOverTime());

        _movementSpeedAtoms.Changed.Register(OnMovementChange);
        _dashSpeedAtoms.Changed.Register(OnDashChange);
    }

    private void Update()
    {
        UpdateSmoothInputMovement();
        UpdateDeltaAim();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateTurn();
        UpdateLocalVelocity();
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutineUpdateAimOverTime);

        _movementSpeedAtoms.Changed.Unregister(OnMovementChange);
        _dashSpeedAtoms.Changed.Unregister(OnDashChange);
    }
    #endregion

    private void UpdateLocalVelocity()
    {
        Vector3 currentPosition = transform.position;
        LocalVelocity = transform.InverseTransformDirection((currentPosition - _oldPosition)/ Time.fixedDeltaTime);
        _oldPosition = currentPosition;
        //Debug.Log(LocalVelocity);
    }

    private void UpdateSmoothInputMovement()
    {
        SmoothInputMovement = Vector3.Lerp(SmoothInputMovement, RawInputMovement,
                Time.deltaTime * (IsMoving ? MovementSmoothSpeedStart : MovementSmoothSpeedEnd));
    }

    private void UpdateMovement()
    {
        Vector3 velocity = GetCameraDirection * _movementSpeed * Time.deltaTime;
        _body.MovePosition(transform.position + velocity);
    }

    private void UpdateTurn()
    {
        Quaternion rotation = Quaternion.Slerp(_body.rotation, Quaternion.LookRotation(_aimWorldDirection), TurnSpeed * Time.deltaTime);
        _body.MoveRotation(rotation);
    }

    private void UpdateAim()
    {
        Ray ray = _mainCamera.ScreenPointToRay(AimScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, AimLayerMask))
            AimWorldPosition = hit.point;

        _aimWorldDirection = GetAimDirection;
    }

    // Update aim if aiming using a controller.
    private void UpdateDeltaAim()
    {
        if (_isDeltaAim)
        {
            AimScreenPosition += _deltaAim * DeltaAimSpeed;
            UpdateAim();
        }
    }

    // To prevent loosing aim when not performing any aim movement.
    private IEnumerator UpdateAimOverTime()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            yield return new WaitUntil(() => !IsCameraAngleChanged);    // Only update over time if not changing the camera
            UpdateAim();
            yield return new WaitForSeconds(1.0f);
        }
    }
}
