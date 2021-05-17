using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityAtoms.InputSystem;
using MyBox;
using UnityEngine.Events;

public class UICursorHandler : Singleton<UICursorHandler>
{
    public CursorTypes CursorType => _cursorType;

    #region Inspector
    [Foldout("Atoms", true)]
    [SerializeField] private Vector2Variable _pointScreenPositionVariable;
    [SerializeField] private BoolVariable _isInputVariable;
    [SerializeField] private InputControlSchemeVariable _inputControlSchemeVariable;
    #endregion

    private PlayerControls _controls;
    private Image _image;
    private Coroutine _coroutineUpdatePosition;

    private CursorTypes _cursorType;

    public enum CursorTypes
    {
        Mouse,
        Controller,
    }

    public enum CursorDisplay
    {
        None,
        Mouse,
        Controller,
    }

    #region Events
    public void OnDeviceChanged(PlayerInput input)
    {
        InputControlScheme scheme = input.user.controlScheme.Value;

        _inputControlSchemeVariable.Value = scheme;

        UpdateScheme(scheme);
    }

    public void OnInputChange(bool isInput)
    {
        if (isInput)
        {
            switch (_cursorType)
            {
                case CursorTypes.Controller:
                    SetCursor(CursorDisplay.None);
                    break;
                case CursorTypes.Mouse:
                    SetCursor(CursorDisplay.Mouse);
                    break;
            }
        }
        else
        {
            switch (_cursorType)
            {
                case CursorTypes.Controller:
                    SetCursor(CursorDisplay.Controller);
                    break;
                case CursorTypes.Mouse:
                    SetCursor(CursorDisplay.Mouse);
                    break;
            }
        }
    }
    #endregion

    public void SetCursor(CursorDisplay type)
    {
        switch (type)
        {
            case CursorDisplay.Mouse:
                if (_coroutineUpdatePosition != null)
                    StopCoroutine(_coroutineUpdatePosition);
                _image.enabled = false;
                Cursor.visible = true;
                break;
            case CursorDisplay.Controller:
                if (isActiveAndEnabled)
                    _coroutineUpdatePosition = StartCoroutine(CoroutineUpdatePositon());
                _image.enabled = true;
                Cursor.visible = false;
                break;
            case CursorDisplay.None:
                if (_coroutineUpdatePosition != null)
                    StopCoroutine(_coroutineUpdatePosition);
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
        _isInputVariable.Changed.Register(OnInputChange);
    }

    private void OnEnable()
    {
        _controls.Enable();

        SetStartScheme();
        UpdateScheme(_inputControlSchemeVariable.Value);
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

        _isInputVariable.Changed.Unregister(OnInputChange);
    }

    private void UpdateScheme(InputControlScheme scheme)
    {
        if (scheme == _controls.GamepadScheme)
        {
            _cursorType = CursorTypes.Controller;

            if (_isInputVariable.Value)
            {
                SetCursor(CursorDisplay.None);
            }
            else
                SetCursor(CursorDisplay.Controller);
        }
        else if (scheme == _controls.KeyboardAndMouseScheme)
        {
            _cursorType = CursorTypes.Mouse;

            SetCursor(CursorDisplay.Mouse);
        }
    }

    private void SetStartScheme()
    {
        if (_inputControlSchemeVariable.Value.name == "")  // Give it a start scheme if none has yet been assigned.
        {
            if (Gamepad.all.Count > 0)
                _inputControlSchemeVariable.Value = _controls.GamepadScheme;
            else
                _inputControlSchemeVariable.Value = _controls.KeyboardAndMouseScheme;
        }
    }

    private IEnumerator CoroutineUpdatePositon()
    {
        while (true)
        {
            transform.position = _pointScreenPositionVariable.Value;

            yield return null;
        }
    }
}