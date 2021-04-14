using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;

public class ControllerCursor : MonoBehaviour
{
    [Header("Read References")]
    [SerializeField] private Vector2Variable _cursorScreenPositionAtoms;

    private PlayerControls _controls;
    private Image _image;

    #region Events
    public void OnControlsChanged(PlayerInput input)
    {
        InputControlScheme scheme = input.user.controlScheme.Value;

        if (scheme == _controls.GamepadScheme)
        {
            enabled = true;
            _image.enabled = true;
        }
        else if (scheme == _controls.KeyboardAndMouseScheme)
        {
            enabled = false;
            _image.enabled = false;
        }
    }
    #endregion

    #region Unity Message
    private void Awake()
    {
        _controls = new PlayerControls();
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        transform.position = _cursorScreenPositionAtoms.Value;
    }
    #endregion
}
