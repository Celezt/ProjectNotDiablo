using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;

public class PlayerDataReference : MonoBehaviour
{
    [Header("Variables")]
    public FloatVariable Health;
    public FloatVariable MaxHealth;
    public FloatVariable MovementSpeed;
    [Header("Value Lists")]
    public DurationValueList StunDodgeList;
    public DurationValueList StunMoveList;
    public DurationValueList StunAttackList;
    public DurationValueList InvisibilityFrameList;
    [Header("Events")]
    public AnimatorModifierEvent AnimatorModifierEvent;
    public VoidEvent DieEvent;
}
