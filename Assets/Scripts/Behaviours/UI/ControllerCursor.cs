using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityAtoms.InputSystem;

public class ControllerCursor : MonoBehaviour
{
    [Header("Atoms")]
    [SerializeField] private Vector2Variable _cursorScreenPositionVariable;
    [SerializeField] private PlayerInputEvent _deviceChangedEvent;

    private PlayerControls _input;
    private Image _image;

    #region Events
    public void OnDeviceChanged(PlayerInput input)
    {
        InputControlScheme scheme = input.user.controlScheme.Value;

        if (scheme == _input.GamepadScheme)
        {
            enabled = true;
            _image.enabled = true;
            Cursor.visible = false;
        }
        else if (scheme == _input.KeyboardAndMouseScheme)
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
        _input = new PlayerControls();
        _image = GetComponent<Image>();

        _deviceChangedEvent.Register(OnDeviceChanged);
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void Update()
    {
        transform.position = _cursorScreenPositionVariable.Value;
    }
    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnDestroy()
    {
        _deviceChangedEvent.Unregister(OnDeviceChanged);
    }
    #endregion
}
