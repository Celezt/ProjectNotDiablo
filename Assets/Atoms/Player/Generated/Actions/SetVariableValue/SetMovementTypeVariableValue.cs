using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Set variable value Action of type `MovementType`. Inherits from `SetVariableValue&lt;MovementType, MovementTypePair, MovementTypeVariable, MovementTypeConstant, MovementTypeReference, MovementTypeEvent, MovementTypePairEvent, MovementTypeVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/MovementType", fileName = "SetMovementTypeVariableValue")]
    public sealed class SetMovementTypeVariableValue : SetVariableValue<
        MovementType,
        MovementTypePair,
        MovementTypeVariable,
        MovementTypeConstant,
        MovementTypeReference,
        MovementTypeEvent,
        MovementTypePairEvent,
        MovementTypeMovementTypeFunction,
        MovementTypeVariableInstancer>
    { }
}
