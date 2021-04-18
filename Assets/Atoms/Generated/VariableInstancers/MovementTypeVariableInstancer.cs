using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable Instancer of type `MovementType`. Inherits from `AtomVariableInstancer&lt;MovementTypeVariable, MovementTypePair, MovementType, MovementTypeEvent, MovementTypePairEvent, MovementTypeMovementTypeFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/MovementType Variable Instancer")]
    public class MovementTypeVariableInstancer : AtomVariableInstancer<
        MovementTypeVariable,
        MovementTypePair,
        MovementType,
        MovementTypeEvent,
        MovementTypePairEvent,
        MovementTypeMovementTypeFunction>
    { }
}
