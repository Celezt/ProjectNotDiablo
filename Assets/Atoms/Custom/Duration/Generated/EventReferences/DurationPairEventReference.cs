using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `DurationPair`. Inherits from `AtomEventReference&lt;DurationPair, DurationVariable, DurationPairEvent, DurationVariableInstancer, DurationPairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class DurationPairEventReference : AtomEventReference<
        DurationPair,
        DurationVariable,
        DurationPairEvent,
        DurationVariableInstancer,
        DurationPairEventInstancer>, IGetEvent 
    { }
}
