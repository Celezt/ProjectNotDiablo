using UnityEngine;
using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable of type `MovementType`. Inherits from `AtomVariable&lt;MovementType, MovementTypePair, MovementTypeEvent, MovementTypePairEvent, MovementTypeMovementTypeFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/MovementType", fileName = "MovementTypeVariable")]
    public sealed class MovementTypeVariable : AtomVariable<MovementType, MovementTypePair, MovementTypeEvent, MovementTypePairEvent, MovementTypeMovementTypeFunction>
    {
        protected override bool ValueEquals(MovementType other) => Value == other;
    }
}
