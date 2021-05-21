using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using MyBox;

public class AttackBehaviour : MonoBehaviour
{
    public GameObject SelectedWeapon
    {
        get => _selectedWeapon;
        set
        {
            Transform parentTransform = _selectedWeapon.transform.parent;

            if (!value.name.Contains("HandsWeapon"))
            {
                value.GetComponent<BoxCollider>().enabled = false;
                value.GetComponent<ItemPickupScript>().enabled = false;
                if (!_selectedWeapon.name.Contains("HandsWeapon"))
                {
                    Destroy(_selectedWeapon);

                }
                _selectedWeapon = Instantiate(value, parentTransform.position, parentTransform.rotation, parentTransform); // Instantiate the new weapon.
                _selectedWeapon.transform.localScale = value.transform.localScale;
            }
                              // Destroy the current weapon.
            
        }
    }

    [SerializeField] private GameObject _selectedWeapon;
    [SerializeField] private float _attackCombinationCooldown = 2.0f;
    [Space(10)]
    [SerializeField] private OrderType _meleeOrderType;
    [SerializeField] private AttackData[] _meleeData;
    [SerializeField] private OrderType _rangedOrderType;
    [SerializeField] private AttackData[] _rangedData;

    [Foldout("Atoms", true)]
    [SerializeField] private Vector3Variable _pointWorldPositionVariable;
    [SerializeField] private AnimatorModifierEvent _animatorModifierEvent;
    [SerializeField] private DurationValueList _stunDodgeList;
    [SerializeField] private DurationValueList _stunMoveList;
    [SerializeField] private DurationValueList _stunAttackList;

    private PlayerControls _controls;

    private Coroutine _coroutineUpdateStunned;
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
        void CustomAnimation(AttackData[] dataBuffer, OrderType type)
        {
            if (dataBuffer.Length == 0)     // Skip if no data was found.
                return;

            int index = 0;

            switch (type)
            {
                case OrderType.Random:
                    index = UnityEngine.Random.Range(0, dataBuffer.Length);
                    break;
                case OrderType.Sequence:
                    if (!_attackCombinationDuration.IsActive)
                        _attackIndex = 0;

                    index = _attackIndex;

                    _attackIndex = (_attackIndex + 1) % dataBuffer.Length;

                    _attackCombinationDuration = new Duration(_attackCombinationCooldown);
                    break;
            }

            AttackData data = dataBuffer[index];

            int clipIndex = UnityEngine.Random.Range(0, data.Clip.Length);  // Pick random clip.

            _stunDodgeList.Add(new Duration(data.Clip[clipIndex].length / data.AnimationSpeedMultiplier * data.StunDodgeMultiplier));
            _stunMoveList.Add(new Duration(data.Clip[clipIndex].length / data.AnimationSpeedMultiplier * data.StunMoveMultiplier));
            _animatorModifierEvent.Raise(new AnimatorModifier(data.Clip[clipIndex], data.AnimationSpeedMultiplier));
        }

        Melee melee = _selectedWeapon.GetComponent<Melee>();
        if (melee != null && melee.cooldownTimer <= 0)
        {
            melee.Attack(transform, gameObject.GetComponent<Collider>());


            CustomAnimation(_meleeData, _meleeOrderType);
        }

        Ranged ranged = _selectedWeapon.GetComponent<Ranged>();
        if (ranged != null && ranged.cooldownTimer <= 0)
        {
            ranged.Attack(_pointWorldPositionVariable.Value);

            CustomAnimation(_rangedData, _rangedOrderType);
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
        _controls.Ground.Attack.performed += OnAttack;
        _controls.Enable();

        _coroutineUpdateStunned = StartCoroutine(UpdateStunned());
    }

    private void OnDisable()
    {
        _controls.Ground.Attack.performed -= OnAttack;
        _controls.Disable();

        StopCoroutine(_coroutineUpdateStunned);
    }
    #endregion

    private IEnumerator UpdateStunned()
    {
        yield return new WaitForFixedUpdate();

        while (true)
        {
            for (int i = 0; i < _stunAttackList.Count; i++)
            {
                Duration duration = _stunAttackList[i];

                if (!duration.IsActive)
                    _stunAttackList.Remove(duration);
            }

            if (_stunAttackList.Count != 0)
            {
                _controls.Disable();
            }
            else
            {
                _controls.Enable();
            }

            yield return new WaitForUnscaledSeconds(0.1f);
        }
    }
}
