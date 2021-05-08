using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityAtoms.InputSystem;
using MyBox;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private Sprite _mouseCursorTexture;
    [Foldout("Atoms")]
    [SerializeField] private Vector2Variable _pointScreenPositionVariable;
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

    private void Start()
    {
        //Cursor.SetCursor(_mouseCursorTexture.texture, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void Update()
    {
        transform.position = _pointScreenPositionVariable.Value;
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
