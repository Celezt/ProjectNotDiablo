using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityAtoms.BaseAtoms;
using UnityAtoms.InputSystem;
using MyBox;

public class UIMainMenu : Singleton<MonoBehaviour>
{
    [SerializeField] private SceneReference _newGameScene;
    [SerializeField] private GameObject _firstButton;

    [Foldout("Atoms", true)]
    [SerializeField] private BoolVariable _isInputVariable;

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
    public void OnNewGame() => _newGameScene.LoadScene();

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

        _isInputVariable.Value = true;

        PlayerInput.GetPlayerByIndex(0).SwitchCurrentActionMap(_controls.UI.Get().name);
        PlayerInput.GetPlayerByIndex(0).controlsChangedEvent.AddListener(OnDeviceChanged);
    }

    private void OnDisable()
    {
        _controls.Disable();

        _isInputVariable.Value = false;

        if (PlayerInput.GetPlayerByIndex(0) != null)
        {
            PlayerInput.GetPlayerByIndex(0).SwitchCurrentActionMap(_controls.Ground.Get().name);
            PlayerInput.GetPlayerByIndex(0).controlsChangedEvent.RemoveListener(OnDeviceChanged);
        }
    }
    #endregion
}
