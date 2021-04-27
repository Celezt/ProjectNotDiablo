using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;

public class PlayerStats : MonoBehaviour
{
    public PlayerControls Controls
    {
        get => _controls;
    }

    #region Inspector
    [Header("Atoms")]
    [SerializeField] private FloatVariable _healthVariable;
    #endregion

    private PlayerControls _controls;

    #region Events
    public void OnHotbar(InputAction.CallbackContext context)
    {
        switch (context.ReadValue<float>())
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            default:
                break;
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
        _controls.Ground.Hotbar.performed += OnHotbar;
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Ground.Hotbar.performed -= OnHotbar;
        _controls.Disable();
    }
    #endregion
}
