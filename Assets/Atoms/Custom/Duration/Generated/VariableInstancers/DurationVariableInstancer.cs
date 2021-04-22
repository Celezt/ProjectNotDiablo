using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable Instancer of type `Duration`. Inherits from `AtomVariableInstancer&lt;DurationVariable, DurationPair, Duration, DurationEvent, DurationPairEvent, DurationDurationFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/Duration Variable Instancer")]
    public class DurationVariableInstancer : AtomVariableInstancer<
        DurationVariable,
        DurationPair,
        Duration,
        DurationEvent,
        DurationPairEvent,
        DurationDurationFunction>
    { }
}
