using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using MyBox;

public class PointBehaviour : MonoBehaviour
{
    public PlayerControls Controls
    {
        get => _controls;
    }

    public Vector3 GetPointDirection
    {
        get
        {
            Vector3 playerPosition = transform.position;

            return (_pointWorldPosition != Vector3.zero) ? new Vector3(_pointWorldPosition.x - playerPosition.x, 0, _pointWorldPosition.z - playerPosition.z) : Vector3.forward;
        }
    }

    #region Inspector
    [SerializeField] private Transform _cameraPivotTransform;
    [Space(10)]
    [Header("Settings")]
    [SerializeField, Tooltip("Only for controllers")] private FloatReference _deltaCursorSpeedReference;
    [SerializeField] private LayerMask _aimLayerMask;
    [Foldout("Atoms", true)]
    [SerializeField] private Vector2Variable _pointScreenPositionVariable;
    [SerializeField] private Vector3Variable _pointWorldDirectionVariable;
    [SerializeField] private Vector3Variable _pointWorldPositionVariable;
    #endregion

    private Camera _mainCamera;
    private PlayerControls _controls;

    private Coroutine _coroutineUpdateAimOverTime;

    private Plane _plane = new Plane(Vector3.up, 0);

    private Vector3 _pointWorldPosition;
    private Vector2 _pointScreenPosition;
    private Vector2 _pointDelta;
    private bool _isPointDelta;
    private bool _isCameraAngleChanged;

    #region Events
    public void OnPointPosition(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = context.ReadValue<Vector2>();
        if (screenPosition != Vector2.zero)
            _pointScreenPosition = context.ReadValue<Vector2>();

        _pointScreenPositionVariable.Value = _pointScreenPosition;

        UpdatePoint();
    }

    public void OnPointDelta(InputAction.CallbackContext context)
    {
        _pointDelta = context.ReadValue<Vector2>();
        _isPointDelta = (_pointDelta != Vector2.zero);
    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        _isCameraAngleChanged = (context.ReadValue<Vector2>() != Vector2.zero);
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
    }

    private void Update()
    {
        UpdatePointDelta();
    }

    private void OnEnable()
    {
        _coroutineUpdateAimOverTime = StartCoroutine(UpdatePointOverTime());

        _controls.Ground.Point.performed += OnPointPosition;
        _controls.Ground.Point.canceled += OnPointPosition;
        _controls.Ground.PointDelta.performed += OnPointDelta;
        _controls.Ground.PointDelta.canceled += OnPointDelta;
        _controls.Ground.Camera.performed += OnCamera;
        _controls.Ground.Camera.canceled += OnCamera;
        _controls.Enable();
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutineUpdateAimOverTime);

        _controls.Ground.Point.performed -= OnPointPosition;
        _controls.Ground.Point.canceled -= OnPointPosition;
        _controls.Ground.PointDelta.performed -= OnPointDelta;
        _controls.Ground.PointDelta.canceled -= OnPointDelta;
        _controls.Ground.Camera.performed -= OnCamera;
        _controls.Ground.Camera.canceled -= OnCamera;
        _controls.Disable();
    }
    #endregion

    private void UpdatePoint()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_pointScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _aimLayerMask))
            _pointWorldPositionVariable.Value = _pointWorldPosition = hit.point;
        else if (_plane.Raycast(ray, out float distance))   // Collide with a plane if no object was collided with.
            _pointWorldPositionVariable.Value = _pointWorldPosition = ray.GetPoint(distance);

        _pointWorldDirectionVariable.Value = GetPointDirection;
    }

    // Update aim if aiming using a controller.
    private void UpdatePointDelta()
    {
        if (_isPointDelta)
        {
            _pointScreenPosition += _pointDelta * Time.deltaTime * _deltaCursorSpeedReference.Value;
            _pointScreenPositionVariable.Value = _pointScreenPosition;

            UpdatePoint();
        }
    }

    // To prevent loosing aim when not performing any aim movement.
    private IEnumerator UpdatePointOverTime()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            yield return new WaitUntil(() => !_isCameraAngleChanged);    // Only update over time if not changing the camera
            UpdatePoint();
            yield return new WaitForSeconds(1.0f);
        }
    }

}
