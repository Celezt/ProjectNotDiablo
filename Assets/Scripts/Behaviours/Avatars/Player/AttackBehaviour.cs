using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private Vector3Variable _pointWorldPositionVariable;

    [SerializeField] private GameObject _selectedWeapon;

    [SerializeField] private AnimationClip _attackClip;

    private PlayerControls _controls;

    private AnimatorBehaviour _animatorBehaviour;

    #region Events
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (_selectedWeapon.GetComponent<Ranged>() != null)
        {
            _selectedWeapon.GetComponent<Ranged>().Attack(_pointWorldPositionVariable.Value);
        }
        if (_selectedWeapon.GetComponent<Melee>() != null)
        {
            _selectedWeapon.GetComponent<Melee>().Attack(transform);

            if (_animatorBehaviour.IsAnimationModifierRunning == false)
            {
                _animatorBehaviour.OnAnimationModifierRaised(new AnimatorModifier(_attackClip));
            }
        }
    }
    #endregion

    #region Unity Message
    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void Start()
    {
        _animatorBehaviour = GetComponent<AnimatorBehaviour>();
    }

    private void OnEnable()
    {
        _controls.Ground.Attack.performed += OnAttack;
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Ground.Attack.performed -= OnAttack;
        _controls.Disable();
    }
    #endregion
}
