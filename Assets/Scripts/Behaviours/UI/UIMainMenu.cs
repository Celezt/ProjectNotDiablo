using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityAtoms.BaseAtoms;
using UnityAtoms;
using UnityAtoms.InputSystem;
using MyBox;

public class UIMainMenu : Singleton<MonoBehaviour>
{
    [SerializeField] private SceneReference _newGameScene;
    [SerializeField] private GameObject _firstButton;

    [Foldout("Atoms", true)]
    [SerializeField] private BoolVariable _isInputVariable;
    [SerializeField] private InputControlSchemeVariable _inputControlSchemeVariable;

    [SerializeField] private BaseAtomValueList[] _valueLists;
    [SerializeField] private FloatVariable _playerHealth;
    [SerializeField] private FloatVariable _playerMaxHealth;

    private PlayerControls _controls;

    #region Events
    /// <summary>
    /// Quit the game (works in editor mode).
    /// </summary>
    public void OnQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Start a new game.
    /// </summary>
    public void OnNewGame()
    {
        if (_valueLists != null)
        {
            for (int i = 0; i < _valueLists.Length; i++)  // Clear all variable lists.
            {
                _valueLists[i].Clear();
            }
        }

        if (_playerHealth != null && _playerMaxHealth != null)
        {
            _playerHealth.Value = _playerMaxHealth.Value;
        }

        _newGameScene.LoadScene();
    }

    public void OnDeviceChanged(PlayerInput input)
    {
        InputControlScheme scheme = input.user.controlScheme.Value;

        if (scheme == _controls.GamepadScheme)
        {
            SelectFirstObject();
        }
        else if (scheme == _controls.KeyboardAndMouseScheme)
        {
            DeselectFirstObject();
        }
    }
    #endregion

    public void SelectFirstObject() => EventSystem.current.SetSelectedGameObject(_firstButton);
    public void DeselectFirstObject() => EventSystem.current.SetSelectedGameObject(null);

    #region Unity Message
    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void OnEnable()
    {
        _controls.Enable();

        if (_inputControlSchemeVariable.Value == _controls.GamepadScheme)
            SelectFirstObject();

        _isInputVariable.Value = true;

        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(0);
        playerInput.SwitchCurrentActionMap(_controls.UI.Get().name);
        playerInput.controlsChangedEvent.AddListener(OnDeviceChanged);
    }

    private void OnDisable()
    {
        _controls.Disable();

        _isInputVariable.Value = false;

        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(0);
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap(_controls.Ground.Get().name);
            playerInput.controlsChangedEvent.RemoveListener(OnDeviceChanged);
        }
    }
    #endregion
}
