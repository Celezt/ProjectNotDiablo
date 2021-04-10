using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float MovementSpeed = 5.0f;
    public float MovementSmoothSpeed = 1.0f;
    public float TurnSpeed = 0.1f;
    public float TurnStartMargin = 0.01f;
    public float TurnEndMargin = 0.2f;

    public bool IsMoving { get; set; }

    private Camera _mainCamera;
    private Rigidbody _body;

    private Vector3 _rawInputMovement;
    private Vector3 _smoothInputMovement;

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

            return cameraForward * _smoothInputMovement.z + cameraRight * _smoothInputMovement.x;
        }
    }

#region Events
    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        _rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);

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
        _smoothInputMovement = Vector3.Lerp(_smoothInputMovement, _rawInputMovement, Time.deltaTime * MovementSmoothSpeed);
    }

    /// <summary>
    /// Move using rigid-body.
    /// </summary>
    private void Move()
    {
        Vector3 movement = GetCameraDirection * MovementSpeed * Time.deltaTime;
        _body.MovePosition(transform.position + movement);
    }

    /// <summary>
    /// Turn the player around its axis using rigid-body.
    /// </summary>
    private void Turn()
    {
        if ((IsMoving && _smoothInputMovement.sqrMagnitude > TurnStartMargin) || (!IsMoving && _smoothInputMovement.sqrMagnitude > TurnEndMargin))
        {
            Quaternion rotation = Quaternion.Slerp(_body.rotation, Quaternion.LookRotation(GetCameraDirection), TurnSpeed);
            _body.MoveRotation(rotation);
        }
    }
}
