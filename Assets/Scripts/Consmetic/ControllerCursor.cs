using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;

public class ControllerCursor : MonoBehaviour
{
    [Header("Atoms")]
    [SerializeField] private Vector2Variable _cursorScreenPositionAtoms;
    [SerializeField] private PlayerInput _playerInput;

    private PlayerControls _input;
    private Image _image;

    #region Events
    public void OnControlsChanged(PlayerInput input)
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
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void Update()
    {
        transform.position = _cursorScreenPositionAtoms.Value;
    }
    private void OnDisable()
    {
        _input.Disable();
    }
    #endregion
}
