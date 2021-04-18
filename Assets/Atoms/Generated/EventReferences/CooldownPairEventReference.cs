using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `CooldownPair`. Inherits from `AtomEventReference&lt;CooldownPair, CooldownVariable, CooldownPairEvent, CooldownVariableInstancer, CooldownPairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class CooldownPairEventReference : AtomEventReference<
        CooldownPair,
        CooldownVariable,
        CooldownPairEvent,
        CooldownVariableInstancer,
        CooldownPairEventInstancer>, IGetEvent 
    { }
}
