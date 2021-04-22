using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `Duration`. Inherits from `AtomEventReference&lt;Duration, DurationVariable, DurationEvent, DurationVariableInstancer, DurationEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class DurationEventReference : AtomEventReference<
        Duration,
        DurationVariable,
        DurationEvent,
        DurationVariableInstancer,
        DurationEventInstancer>, IGetEvent 
    { }
}
