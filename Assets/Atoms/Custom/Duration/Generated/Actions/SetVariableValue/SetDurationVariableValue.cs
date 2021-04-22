using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Set variable value Action of type `Duration`. Inherits from `SetVariableValue&lt;Duration, DurationPair, DurationVariable, DurationConstant, DurationReference, DurationEvent, DurationPairEvent, DurationVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/Duration", fileName = "SetDurationVariableValue")]
    public sealed class SetDurationVariableValue : SetVariableValue<
        Duration,
        DurationPair,
        DurationVariable,
        DurationConstant,
        DurationReference,
        DurationEvent,
        DurationPairEvent,
        DurationDurationFunction,
        DurationVariableInstancer>
    { }
}
