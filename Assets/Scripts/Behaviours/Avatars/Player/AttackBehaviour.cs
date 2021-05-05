using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using MyBox;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _selectedWeapon;

    [SerializeField] private AnimationClip _attackClip;
    [SerializeField] private float _animationSpeed = 1.0f;
    [SerializeField] private float _stunMoveMultiplier = 1.0f;
    [SerializeField] private float _stunDodgeMultiplier = 1.0f;

    [Foldout("Atoms", true)]
    [SerializeField] private Vector3Variable _pointWorldPositionVariable;
    [SerializeField] private DurationValueList _stunDodgeList;
    [SerializeField] private DurationValueList _stunMoveList;

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
            LayerMask selfLayer = LayerMask.GetMask("Player");
            _selectedWeapon.GetComponent<Melee>().Attack(transform, selfLayer);

            if (_animatorBehaviour.IsAnimationModifierRunning == false)
            {
                _stunDodgeList.Add(new Duration(_attackClip.length / _animationSpeed * _stunDodgeMultiplier));
                _stunMoveList.Add(new Duration(_attackClip.length / _animationSpeed * _stunMoveMultiplier));
                _animatorBehaviour.OnAnimationModifierRaised(new AnimatorModifier(_attackClip, _animationSpeed));
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
