using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityAtoms.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using MyBox;

public class UIKeyIcon : MonoBehaviour
{
    [SerializeField] private KeyIconScriptableObject _keyIcons;
    [SerializeField] private KeyIconScriptableObject.KeyIcons.Keys _key;
    [SerializeField] private bool _hasText;
    [SerializeField, ConditionalField(nameof(_hasText))] private TextAlignment _alignment;
    [SerializeField, ConditionalField(nameof(_hasText))] private string _content;


    private Image _image;
    private Text _text;

    private PlayerControls _controls;

    public enum TextAlignment
    {
        Left,
        Right,
        Up,
        Down,
    }

    #region Events
    public void OnDeviceChanged(PlayerInput input)
    {
        InputControlScheme scheme = input.user.controlScheme.Value;

        if (scheme == _controls.GamepadScheme)
        {
            if (Gamepad.current is DualShockGamepad)
            {
                _image.sprite = _keyIcons.Ps4.GetSprite(_key);
            }
            else if (Gamepad.current is XInputController)
            {
                _image.sprite = _keyIcons.Xbox.GetSprite(_key);
            }
        }
        else if (scheme == _controls.KeyboardAndMouseScheme)
        {
            _image.sprite = _keyIcons.Pc.GetSprite(_key);
        }
    }
    #endregion

    #region Unity Message
    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void Start()
    {
        _image = GetComponentInChildren<Image>();
        _text = GetComponentInChildren<Text>();

        SetText();

        OnDeviceChanged(PlayerInput.GetPlayerByIndex(0));
    }

    private void OnEnable()
    {
        _controls.Enable();
        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(0);
        playerInput.controlsChangedEvent.AddListener(OnDeviceChanged);
    }

    private void OnDisable()
    {
        _controls.Disable();
        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(0);
        if (playerInput != null)
            playerInput.controlsChangedEvent.AddListener(OnDeviceChanged);
    }
    #endregion

    private void SetText()
    {
        if (_text == null || !_hasText)
            return;

        _text.text = _content;

        switch (_alignment)
        {
            case TextAlignment.Left:
                _text.transform.localPosition = new Vector3(-transform.localScale.x * 70, 0, 0);
                _text.alignment = TextAnchor.UpperRight;
                break;
            case TextAlignment.Right:
                _text.transform.localPosition = new Vector3(transform.localScale.x * 70, 0, 0);
                _text.alignment = TextAnchor.UpperLeft;
                break;
            case TextAlignment.Up:
                _text.transform.localPosition = new Vector3(0, transform.localScale.y * 70, 0);
                _text.alignment = TextAnchor.UpperCenter;
                break;
            case TextAlignment.Down:
                _text.transform.localPosition = new Vector3(0, -transform.localScale.y * 70, 0);
                _text.alignment = TextAnchor.UpperCenter;
                break;
        }
    }
}
