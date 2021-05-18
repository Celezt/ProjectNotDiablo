using System;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `AtomEventReference&lt;UnityEngine.InputSystem.InputControlScheme, InputControlSchemeVariable, InputControlSchemeEvent, InputControlSchemeVariableInstancer, InputControlSchemeEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class InputControlSchemeEventReference : AtomEventReference<
        UnityEngine.InputSystem.InputControlScheme,
        InputControlSchemeVariable,
        InputControlSchemeEvent,
        InputControlSchemeVariableInstancer,
        InputControlSchemeEventInstancer>, IGetEvent 
    { }
}
