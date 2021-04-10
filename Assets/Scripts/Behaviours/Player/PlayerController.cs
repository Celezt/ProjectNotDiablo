using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float MovementSpeed = 5.0f;
    [Range(1, 10)] public float MovementSmoothSpeedStart = 2.0f;
    [Range(1, 10)] public float MovementSmoothSpeedEnd = 2.0f;
    public float TurnSpeed = 0.1f;
    public float TurnStartMargin = 0.01f;
    public float TurnEndMargin = 0.2f;

    public bool IsMoving { get; set; }
    public Vector3 RawInputMovement { get; set; }
    public Vector3 SmoothInputMovement { get; set; }
    public Vector3 Velocity { get; set; }

    private Camera _mainCamera;
    private Rigidbody _body;

    /// <summary>
    /// Get direction from the camera.
    /// </summary>
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

    #region Events
    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        RawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);

        IsMoving = (inputMovement != Vector2.zero);
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
    }

    private void Update()
    {
        UpdateSmoothInputMovement();
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }
    #endregion

    /// <summary>
    /// Lerp the smooth input movement based on raw input movement.
    /// </summary>
    private void UpdateSmoothInputMovement()
    {
        SmoothInputMovement = Vector3.Lerp(SmoothInputMovement, RawInputMovement,
            Time.deltaTime * (IsMoving ? MovementSmoothSpeedStart : MovementSmoothSpeedEnd));
    }

    /// <summary>
    /// Move using rigid-body.
    /// </summary>
    private void Move()
    {
        Velocity = GetCameraDirection * MovementSpeed * Time.deltaTime;
        _body.MovePosition(transform.position + Velocity);
    }

    /// <summary>
    /// Turn the player around its axis using rigid-body.
    /// </summary>
    private void Turn()
    {
        if ((IsMoving && SmoothInputMovement.sqrMagnitude > TurnStartMargin) || (!IsMoving && SmoothInputMovement.sqrMagnitude > TurnEndMargin))
        {
            Quaternion rotation = Quaternion.Slerp(_body.rotation, Quaternion.LookRotation(GetCameraDirection), TurnSpeed);
            _body.MoveRotation(rotation);
        }
    }
}
