using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityAtoms.InputSystem;
using MyBox;

public class UIGameplayManager : Singleton<UIGameplayManager>
{
    #region Inspector
    [SerializeField] private Text _controllerLabel;

    [Foldout("Atoms", true)]
    [SerializeField] private FloatEvent _healthEvent;
    #endregion

    private PlayerControls _controller;

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
        PlayerInput.GetPlayerByIndex(0).controlsChangedEvent.AddListener(OnDeviceChanged);
    }

    private void OnDisable()
    {
        _controller.Disable();
        PlayerInput.GetPlayerByIndex(0).controlsChangedEvent.RemoveListener(OnDeviceChanged);
    }
    #endregion
}
