using System.Collections;
using System.Collections.Generic;
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
    [Min(0)] public float TurnSpeed = 0.1f;
    [Header("Aim Settings")]
    public LayerMask AimLayerMask;
    #endregion

    public bool IsMoving { get; set; }
    public bool IsDashing { get; set; }
    public Vector3 RawInputMovement { get; set; }
    public Vector3 SmoothInputMovement { get; set; }

    private Camera _mainCamera;
    private Rigidbody _body;

    private float _movementSpeed;
    private float _dashSpeed;
    private Vector3 _aimPosition;
    private Vector3 _aimDirection;

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

            return (_aimPosition != Vector3.zero) ? new Vector3(_aimPosition.x - playerPosition.x, 0, _aimPosition.z - playerPosition.z) : Vector3.forward;
        }
    }

    #region Events
    public void OnMovementChange(float speed) => _movementSpeed = speed;
    public void OnDashChange(float speed) => _dashSpeed = speed;

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

    public void OnAim(InputAction.CallbackContext value)
    {
        Vector2 screenPositon = value.ReadValue<Vector2>();
        Ray ray = _mainCamera.ScreenPointToRay(screenPositon);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, AimLayerMask))
            _aimPosition = hit.point;

        _aimDirection = GetAimDirection;
    }

    public void OnDeviceLost()
    {

    }

    public void OnDeviceRegained()
    {

    }

    public void OnControlsChanged()
    {

    }
    #endregion

    #region Unity Message
    private void Start()
    {
        _mainCamera = Camera.main;
        _body = GetComponent<Rigidbody>();

        _movementSpeedAtoms.Changed.Register(OnMovementChange);
        _dashSpeedAtoms.Changed.Register(OnDashChange);
    }

    private void Update()
    {
        UpdateSmoothInputMovement();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateTurn();
    }

    private void OnDestroy()
    {
        _movementSpeedAtoms.Changed.Unregister(OnMovementChange);
        _dashSpeedAtoms.Changed.Unregister(OnDashChange);
    }
    #endregion

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
        Quaternion rotation = Quaternion.Slerp(_body.rotation, Quaternion.LookRotation(_aimDirection), TurnSpeed);
        _body.MoveRotation(rotation);
    }
}
