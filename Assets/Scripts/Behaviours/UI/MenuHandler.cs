using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using MyBox;

public class MenuHandler : Singleton<MenuHandler>
{
    private PlayerControls _controls;

    [SerializeField] private GameObject _menuContent;

    [Foldout("Atoms", true)]
    [SerializeField] private DurationValueList _stunAttackList;
    [SerializeField] private DurationValueList _stunDodgeList;
    [SerializeField] private DurationValueList _stunMoveList;
    [SerializeField] private DurationValueList _invisibilityFrameList;

    private Duration _stunAttack;
    private Duration _stunDodge;
    private Duration _stunMove;
    private Duration _invisibilityFrame;

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
    public void OnReturn() => SwapMode();

    /// <summary>
    /// Enable or Disable menu.
    /// </summary>
    /// <param name="context"></param>
    public void OnMenu(InputAction.CallbackContext context) => SwapMode();
    #endregion

    #region Unity Message
    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void OnEnable()
    {
        _controls.Ground.Menu.performed += OnMenu;
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Ground.Menu.performed -= OnMenu;
        _controls.Disable();
    }
    #endregion

    private void SwapMode()
    {
        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(0);

        if (playerInput == null)
            return;

        if (playerInput.currentActionMap.name == _controls.Ground.Get().name)
        {
            Duration duration = new Duration(float.MaxValue);
            _stunAttackList?.Add(_stunAttack = duration);
            _stunDodgeList?.Add(_stunDodge = duration);
            _stunMoveList?.Add(_stunMove = duration);
            _invisibilityFrameList?.Add(_invisibilityFrame = duration);

            _menuContent.SetActive(true);

            Time.timeScale = 0.0f;
            playerInput.SwitchCurrentActionMap(_controls.UI.Get().name);
        }
        else if (playerInput.currentActionMap.name == _controls.UI.Get().name)
        {
            _stunAttackList?.Remove(_stunAttack);
            _stunDodgeList?.Remove(_stunDodge);
            _stunMoveList?.Remove(_stunMove);
            _invisibilityFrameList?.Remove(_invisibilityFrame);

            _menuContent.SetActive(false);

            Time.timeScale = 1.0f;
            playerInput.SwitchCurrentActionMap(_controls.Ground.Get().name);
        }
    }
}
