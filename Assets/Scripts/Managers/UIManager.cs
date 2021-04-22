using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityAtoms.InputSystem;

public class UIManager : Singleton<UIManager>
{
    #region Inspector
    [Header("Atoms")]
    [SerializeField] private FloatEvent _healthEvent;
    [SerializeField] private PlayerInputEvent _deviceChangedEvent;
    [Space(10)]
    [Header("UI Setup")]
    [SerializeField] private Text _controllerLabel;
    [SerializeField] private Text _healthLabel;
    #endregion

    private PlayerControls _input;

    protected UIManager() { }

    #region Events
    public void OnHealthChange(float health) => _healthLabel.text = health.ToString();
    public void OnDeviceChanged(PlayerInput input)
    {
        InputControlScheme scheme = input.user.controlScheme.Value;

        if (scheme == _input.GamepadScheme)
        {
            _controllerLabel.text = "Controller";
        }
        else if (scheme == _input.KeyboardAndMouseScheme)
        {
            _controllerLabel.text = "PC";
        }
    }
    #endregion

    #region Unity Message
    private void Awake()
    {
        _input = new PlayerControls();
    }

    private void OnEnable()
    {
        _input.Enable();
        _deviceChangedEvent.Register(OnDeviceChanged);
        _healthEvent.Register(OnHealthChange);
    }

    private void OnDisable()
    {
        _input.Disable();
        _deviceChangedEvent.Unregister(OnDeviceChanged);
        _healthEvent.Unregister(OnHealthChange);
    }
    #endregion
}
