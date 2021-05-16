using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityAtoms.InputSystem;
using MyBox;

public class CursorHandler : Singleton<CursorHandler>
{
    public CursorTypes CursorType => _cursorType;

    #region Inspector
    [SerializeField] private MenuHandler _menuHandler;

    [Foldout("Atoms")]
    [SerializeField] private Vector2Variable _pointScreenPositionVariable;

    private PlayerControls _controls;
    private Image _image;
    #endregion

    private CursorTypes _cursorType;

    public enum CursorTypes
    {
        None,
        Mouse,
        Controller,
    }

    #region Events
    public void OnDeviceChanged(PlayerInput input)
    {
        InputControlScheme scheme = input.user.controlScheme.Value;

        if (scheme == _controls.GamepadScheme)
        {
            if (_menuHandler.IsMenuActive)
            {
                SetCursor(CursorTypes.None);
            }
            else
                SetCursor(CursorTypes.Controller);
        }
        else if (scheme == _controls.KeyboardAndMouseScheme)
        {
            SetCursor(CursorTypes.Mouse);
        }
    }
    #endregion

    public void SetCursor(CursorTypes type)
    {
        _cursorType = type;

        switch (type)
        {
            case CursorTypes.Mouse:
                enabled = false;
                _image.enabled = false;
                Cursor.visible = true;
                break;
            case CursorTypes.Controller:
                enabled = true;
                _image.enabled = true;
                Cursor.visible = false;
                break;
            case CursorTypes.None:
                enabled = false;
                _image.enabled = false;
                Cursor.visible = false;
                break;
        }
    }

    #region Unity Message
    private void Awake()
    {
        _controls = new PlayerControls();
        _image = GetComponent<Image>();

        PlayerInput.GetPlayerByIndex(0).controlsChangedEvent.AddListener(OnDeviceChanged);
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
    #endregion

    public override void Destroy()
    {
        if (PlayerInput.GetPlayerByIndex(0) != null)
            PlayerInput.GetPlayerByIndex(0).controlsChangedEvent.RemoveListener(OnDeviceChanged);
    }
}
