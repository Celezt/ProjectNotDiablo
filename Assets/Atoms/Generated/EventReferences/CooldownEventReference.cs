using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `Cooldown`. Inherits from `AtomEventReference&lt;Cooldown, CooldownVariable, CooldownEvent, CooldownVariableInstancer, CooldownEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class CooldownEventReference : AtomEventReference<
        Cooldown,
        CooldownVariable,
        CooldownEvent,
        CooldownVariableInstancer,
        CooldownEventInstancer>, IGetEvent 
    { }
}
