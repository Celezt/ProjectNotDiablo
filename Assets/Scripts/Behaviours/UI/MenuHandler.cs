using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityAtoms.BaseAtoms;
using MyBox;

public class MenuHandler : Singleton<MenuHandler>
{
    public bool IsMenuActive => _isMenuActive;

    private PlayerControls _controls;

    [SerializeField] private GameObject _menuContent;
    [SerializeField] private GameObject _firstButton;
    [SerializeField] private CursorHandler _cursorHandler;

    [Foldout("Atoms", true)]
    [SerializeField] private DurationValueList _stunAttackList;
    [SerializeField] private DurationValueList _stunDodgeList;
    [SerializeField] private DurationValueList _stunMoveList;
    [SerializeField] private DurationValueList _invisibilityFrameList;

    private Duration _EmptyDuration;

    private bool _isMenuActive;

    #region Events
    /// <summary>
    /// Quit the game (works in editor mode).
    /// </summary>
    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Return back to the game.
    /// </summary>
    public void OnReturn() => ToggleGameplay(PlayerInput.GetPlayerByIndex(0));

    /// <summary>
    /// Enable or Disable menu.
    /// </summary>
    /// <param name="context"></param>
    public void OnMenu(InputAction.CallbackContext context) => ToggleMenuActive();

    public void OnDeviceChanged(PlayerInput input)
    {
        InputControlScheme scheme = input.user.controlScheme.Value;

        if (scheme == _controls.GamepadScheme)
        {
            if (_isMenuActive)
                SelectFirstObject();
        }
        else if (scheme == _controls.KeyboardAndMouseScheme)
        {
            if (_isMenuActive)
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

        _menuContent.SetActive(false);  // Disable menu on start.
    }

    private void OnEnable()
    {
        _controls.Ground.Menu.performed += OnMenu;
        _controls.Enable();

        PlayerInput.GetPlayerByIndex(0).controlsChangedEvent.AddListener(OnDeviceChanged);
    }

    private void OnDisable()
    {
        _controls.Ground.Menu.performed -= OnMenu;
        _controls.Disable();

        PlayerInput.GetPlayerByIndex(0).controlsChangedEvent.RemoveListener(OnDeviceChanged);
    }
    #endregion

    private void ToggleMenuActive()
    {
        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(0);

        if (playerInput == null)
            return;

        string currentActionMap = playerInput.currentActionMap.name;
        if (currentActionMap == "Ground")
            ToggleMenu(playerInput);
        else if (currentActionMap == "UI")
            ToggleGameplay(playerInput);
    }

    private void ToggleMenu(PlayerInput playerInput)
    {
        _isMenuActive = true;

        _EmptyDuration = Duration.Empty;
        _stunAttackList?.Add(_EmptyDuration);
        _stunDodgeList?.Add(_EmptyDuration);
        _stunMoveList?.Add(_EmptyDuration);
        _invisibilityFrameList?.Add(_EmptyDuration);


        _menuContent.SetActive(true);

        if (_cursorHandler.CursorType == CursorHandler.CursorTypes.Controller)
            SelectFirstObject();

        _cursorHandler.SetCursor(CursorHandler.CursorTypes.None);

        Time.timeScale = 0.0f;
        playerInput.SwitchCurrentActionMap("UI");
    }

    private void ToggleGameplay(PlayerInput playerInput)
    {
        _isMenuActive = false;

        _stunAttackList?.Remove(_EmptyDuration);
        _stunDodgeList?.Remove(_EmptyDuration);
        _stunMoveList?.Remove(_EmptyDuration);
        _invisibilityFrameList?.Remove(_EmptyDuration);

        DeselectFirstObject();
        _menuContent.SetActive(false);

        Time.timeScale = 1.0f;
        playerInput.SwitchCurrentActionMap("Ground");
    }
}
