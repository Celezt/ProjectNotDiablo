using System;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `InputControlSchemePair`. Inherits from `AtomEventReference&lt;InputControlSchemePair, InputControlSchemeVariable, InputControlSchemePairEvent, InputControlSchemeVariableInstancer, InputControlSchemePairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class InputControlSchemePairEventReference : AtomEventReference<
        InputControlSchemePair,
        InputControlSchemeVariable,
        InputControlSchemePairEvent,
        InputControlSchemeVariableInstancer,
        InputControlSchemePairEventInstancer>, IGetEvent 
    { }
}
