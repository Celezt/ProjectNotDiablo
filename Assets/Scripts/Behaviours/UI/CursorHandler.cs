using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityAtoms.InputSystem;
using MyBox;

public class CursorHandler : Singleton<MonoBehaviour>
{
    [Foldout("Atoms")]
    [SerializeField] private Vector2Variable _pointScreenPositionVariable;
    [SerializeField] private PlayerInputEvent _deviceChangedEvent;

    private PlayerControls _controls;
    private Image _image;

    #region Events
    public void OnDeviceChanged(PlayerInput input)
    {
        InputControlScheme scheme = input.user.controlScheme.Value;

        if (scheme == _controls.GamepadScheme)
        {
            enabled = true;
            _image.enabled = true;
            Cursor.visible = false;
        }
        else if (scheme == _controls.KeyboardAndMouseScheme)
        {
            enabled = false;
            _image.enabled = false;
            Cursor.visible = true;
        }
    }
    #endregion

    #region Unity Message
    private void Awake()
    {
        _controls = new PlayerControls();
        _image = GetComponent<Image>();

        _deviceChangedEvent.Register(OnDeviceChanged);
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void Update()
    {
        transform.position = _pointScreenPositionVariable.Value;
    }
    private void OnDisable()
    {
        _controls.Disable();
    }

    private void OnDestroy()
    {
        _deviceChangedEvent.Unregister(OnDeviceChanged);
    }
    #endregion
}
