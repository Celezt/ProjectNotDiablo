using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;

public class MenuHandler : Singleton<MenuHandler>
{
    private PlayerControls _controls;

    [SerializeField] private DurationValueList _stunAttackList;

    private Duration _stunAttack;

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
    public void OnReturn()
    {

    }

    /// <summary>
    /// Enable or Disable menu.
    /// </summary>
    /// <param name="context"></param>
    public void OnMenu(InputAction.CallbackContext context)
    {
        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(0);

        if (playerInput == null)
            return;

        if (playerInput.currentActionMap.name == _controls.Ground.Get().name)
        {
            //_stunAttackList.Add(_stunAttack = new Duration(float.MaxValue));
            //Time.timeScale = 0.0f;
            //playerInput.SwitchCurrentActionMap(_controls.UI.Get().name);
            //playerInput.DeactivateInput();
        }
        else if (playerInput.currentActionMap.name == _controls.UI.Get().name)
        {
            //_stunAttackList.Remove(_stunAttack);
            //Time.timeScale = 1.0f;
            //playerInput.SwitchCurrentActionMap(_controls.Ground.Get().name);
            //playerInput.ActivateInput();
        }

    }
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
}
