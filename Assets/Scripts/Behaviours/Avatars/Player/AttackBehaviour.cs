using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using MyBox;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _selectedWeapon;
    [SerializeField] private float _attackCombinationCooldown = 2.0f;
    [Space(10)]
    [SerializeField] private OrderType _meleeOrderType;
    [SerializeField] private AttackData[] _meleeData;
    [SerializeField] private OrderType _rangedOrderType;
    [SerializeField] private AttackData[] _rangedData;

    [Foldout("Atoms", true)]
    [SerializeField] private Vector3Variable _pointWorldPositionVariable;
    [SerializeField] private DurationValueList _stunDodgeList;
    [SerializeField] private DurationValueList _stunMoveList;

    private PlayerControls _controls;

    private AnimatorBehaviour _animatorBehaviour;

    private Duration _attackCombinationDuration;

    private int _attackIndex;

    [Serializable]
    private struct AttackData
    {
        public AnimationClip[] Clip;
        public float AnimationSpeedMultiplier;
        public float StunMoveMultiplier;
        public float StunDodgeMultiplier;
    }

    private enum OrderType
    {
        Random,
        Sequence,
    }

    #region Events
    public void OnAttack(InputAction.CallbackContext context)
    {
        Melee melee = _selectedWeapon.GetComponent<Melee>();
        if (melee != null)
        {
            melee.Attack(transform);

            if (!_animatorBehaviour.IsAnimationModifierRunning)
            {
                int index = 0;

                if (_meleeOrderType == OrderType.Random)
                    index = UnityEngine.Random.Range(0, _meleeData.Length);
                else if (_meleeOrderType == OrderType.Sequence)
                {
                    if (!_attackCombinationDuration.IsActive)
                        _attackIndex = 0;

                    index = _attackIndex;

                    _attackIndex = (_attackIndex + 1) % _meleeData.Length;

                    _attackCombinationDuration = new Duration(_attackCombinationCooldown);
                }

                AttackData data = _meleeData[index];

                int clipIndex = UnityEngine.Random.Range(0, data.Clip.Length);  // Pick random clip.

                _stunDodgeList.Add(new Duration(data.Clip[clipIndex].length / data.AnimationSpeedMultiplier * data.StunDodgeMultiplier));
                _stunMoveList.Add(new Duration(data.Clip[clipIndex].length / data.AnimationSpeedMultiplier * data.StunMoveMultiplier));
                _animatorBehaviour.OnAnimationModifierRaised(new AnimatorModifier(data.Clip[clipIndex], data.AnimationSpeedMultiplier));
            }
        }

        Ranged ranged = _selectedWeapon.GetComponent<Ranged>();
        if (ranged != null)
        {
            ranged.Attack(_pointWorldPositionVariable.Value);

            if (!_animatorBehaviour.IsAnimationModifierRunning)
            {
                int index = 0;

                if (_rangedOrderType == OrderType.Random)
                    index = UnityEngine.Random.Range(0, _rangedData.Length);
                else if (_rangedOrderType == OrderType.Sequence)
                {
                    if (!_attackCombinationDuration.IsActive)
                        _attackIndex = 0;

                    index = _attackIndex;

                    _attackIndex = (_attackIndex + 1) % _rangedData.Length;

                    _attackCombinationDuration = new Duration(_attackCombinationCooldown);
                }

                AttackData data = _rangedData[index];

                int clipIndex = UnityEngine.Random.Range(0, data.Clip.Length);  // Pick random clip.

                _stunDodgeList.Add(new Duration(data.Clip[clipIndex].length / data.AnimationSpeedMultiplier * data.StunDodgeMultiplier));
                _stunMoveList.Add(new Duration(data.Clip[clipIndex].length / data.AnimationSpeedMultiplier * data.StunMoveMultiplier));
                _animatorBehaviour.OnAnimationModifierRaised(new AnimatorModifier(data.Clip[clipIndex], data.AnimationSpeedMultiplier));
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
