using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityAtoms.BaseAtoms;
using MyBox;
using UnityEngine.UI;

public class UIMenuHandler : Singleton<UIMenuHandler>
{
    [Header("References")]
    [SerializeField] private SceneReference _newStartScreen;
    [SerializeField] private GameObject _menuContent;
    [SerializeField] private GameObject _deathMenuContent;
    [SerializeField] private GameObject _gameplayContent;
    [SerializeField] private GameObject _firstMenuButton;
    [SerializeField] private GameObject _firstDeathMenuButton;
    [SerializeField] private Text _timeText;
    [SerializeField] private UICursorHandler _cursorHandler;

    [Header("OnDeath")]
    [SerializeField] private float _deathDelay = 3;

    [Foldout("Atoms", true)]
    [SerializeField] private BoolVariable _isInputVariable;
    [SerializeField] private DurationValueList _stunAttackList;
    [SerializeField] private DurationValueList _stunDodgeList;
    [SerializeField] private DurationValueList _stunMoveList;
    [SerializeField] private DurationValueList _invisibilityFrameList;
    [SerializeField] private VoidEvent _dieEvent;

    private PlayerControls _controls;
    private Duration _EmptyDuration;
    private Stopwatch _gameplayTimer;

    private MenuState _menuState;

    private enum MenuState
    {
        None,
        Menu,
        DeathMenu,
    }

    #region Events
    /// <summary>
    /// Return back to the game.
    /// </summary>
    public void OnReturn() => ToggleGameplay(PlayerInput.GetPlayerByIndex(0));

    /// <summary>
    /// Quit current game session and return to the start screen.
    /// </summary>
    public void OnQuit()
    {
        ToggleGameplay(PlayerInput.GetPlayerByIndex(0));
        _newStartScreen.LoadScene();
    }

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
    /// Enable or Disable menu.
    /// </summary>
    /// <param name="context"></param>
    public void OnMenu(InputAction.CallbackContext context) => ToggleMenuActive();

    /// <summary>
    /// Enable death menu.
    /// </summary>
    public void OnDeathMenu() => StartCoroutine(CoroutineDeath());

    public void OnDeviceChanged(PlayerInput input)
    {
        InputControlScheme scheme = input.user.controlScheme.Value;

        if (scheme == _controls.GamepadScheme)
        {
            switch (_menuState)
            {
                case MenuState.Menu:
                    SelectFirstObject(_firstMenuButton);
                    break;
                case MenuState.DeathMenu:
                    SelectFirstObject(_firstDeathMenuButton);
                    break;
                default:
                    break;
            }
        }
        else if (scheme == _controls.KeyboardAndMouseScheme)
        {
            if (_isInputVariable.Value)
                DeselectFirstObject();
        }
    }
    #endregion

    #region Unity Message
    private void Awake()
    {
        _controls = new PlayerControls();

        _menuContent.SetActive(false);  // Disable menu on start.
        _deathMenuContent.SetActive(false);
        _gameplayContent.SetActive(true);
    }

    private void Start()
    {
        _gameplayTimer = Stopwatch.Initialize();
    }

    private void OnEnable()
    {
        _controls.Ground.Menu.performed += OnMenu;
        _controls.Enable();

        _dieEvent.Register(OnDeathMenu);
        PlayerInput.GetPlayerByIndex(0).controlsChangedEvent.AddListener(OnDeviceChanged);
    }

    private void OnDisable()
    {
        _controls.Ground.Menu.performed -= OnMenu;
        _controls.Disable();

        _dieEvent.Unregister(OnDeathMenu);
        PlayerInput.GetPlayerByIndex(0).controlsChangedEvent.RemoveListener(OnDeviceChanged);
    }
    #endregion

    private void ToggleMenuActive()
    {
        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(0);

        if (playerInput == null)
            return;

        _menuState = MenuState.Menu;

        string currentActionMap = playerInput.currentActionMap.name;
        if (currentActionMap == "Ground")
            ToggleMenu(playerInput);
        else if (currentActionMap == "UI")
            ToggleGameplay(playerInput);
    }

    private void ToggleMenu(PlayerInput playerInput)
    {
        _isInputVariable.Value = true;

        _EmptyDuration = Duration.Infinity;
        _stunAttackList?.Add(_EmptyDuration);
        _stunDodgeList?.Add(_EmptyDuration);
        _stunMoveList?.Add(_EmptyDuration);
        _invisibilityFrameList?.Add(_EmptyDuration);


        _menuContent.SetActive(true);
        _gameplayContent.SetActive(false);

        if (_cursorHandler.CursorType == UICursorHandler.CursorTypes.Controller)
            SelectFirstObject(_firstMenuButton);

        Time.timeScale = 0.0f;
        playerInput.SwitchCurrentActionMap("UI");
    }

    private void ToggleGameplay(PlayerInput playerInput)
    {
        _menuState = MenuState.None;

        _isInputVariable.Value = false;

        _stunAttackList?.Remove(_EmptyDuration);
        _stunDodgeList?.Remove(_EmptyDuration);
        _stunMoveList?.Remove(_EmptyDuration);
        _invisibilityFrameList?.Remove(_EmptyDuration);

        DeselectFirstObject();
        _menuContent.SetActive(false);
        _gameplayContent.SetActive(true);

        Time.timeScale = 1.0f;
        playerInput.SwitchCurrentActionMap("Ground");
    }

    private void SelectFirstObject(GameObject firstButton) => EventSystem.current.SetSelectedGameObject(firstButton);
    private void DeselectFirstObject() => EventSystem.current.SetSelectedGameObject(null);

    private IEnumerator CoroutineDeath()
    {
        yield return new WaitForSeconds(_deathDelay);

        _menuState = MenuState.DeathMenu;

        _controls.Disable();

        TimeSpan timeSpan = TimeSpan.FromSeconds(_gameplayTimer.Timer - _deathDelay);
        _gameplayTimer.Paused();
        _timeText.text = "Play Time: " + timeSpan.ToString(@"hh\:mm\:ss");

        _isInputVariable.Value = true;

        _EmptyDuration = Duration.Infinity;
        _stunAttackList?.Add(_EmptyDuration);
        _stunDodgeList?.Add(_EmptyDuration);
        _stunMoveList?.Add(_EmptyDuration);
        _invisibilityFrameList?.Add(_EmptyDuration);

        _deathMenuContent.SetActive(true);
        _gameplayContent.SetActive(false);

        if (_cursorHandler.CursorType == UICursorHandler.CursorTypes.Controller)
            SelectFirstObject(_firstDeathMenuButton);

        Time.timeScale = 0.0f;
        PlayerInput.GetPlayerByIndex(0).SwitchCurrentActionMap("UI");
    }
}
