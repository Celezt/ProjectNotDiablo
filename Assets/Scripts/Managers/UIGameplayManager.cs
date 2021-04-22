using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityAtoms.InputSystem;

public class UIGameplayManager : Singleton<UIGameplayManager>
{
    public PlayerControls Controls
    {
        get => _controller;
    }

    #region Inspector
    [Header("Atoms")]
    [SerializeField] private FloatEvent _healthEvent;
    [SerializeField] private PlayerInputEvent _deviceChangedEvent;
    [Space(10)]
    [Header("UI Setup")]
    [SerializeField] private Text _controllerLabel;
    #endregion

    private PlayerControls _controller;

    protected UIGameplayManager() { }

    #region Events
    public void OnDeviceChanged(PlayerInput input)
    {
        InputControlScheme scheme = input.user.controlScheme.Value;

        if (scheme == _controller.GamepadScheme)
        {
            _controllerLabel.text = "Controller";
        }
        else if (scheme == _controller.KeyboardAndMouseScheme)
        {
            _controllerLabel.text = "PC";
        }
    }
    #endregion

    #region Unity Message
    private void Awake()
    {
        _controller = new PlayerControls();
    }

    private void OnEnable()
    {
        _controller.Enable();
        _deviceChangedEvent.Register(OnDeviceChanged);
    }

    private void OnDisable()
    {
        _controller.Disable();
        _deviceChangedEvent.Unregister(OnDeviceChanged);
    }
    #endregion
}
