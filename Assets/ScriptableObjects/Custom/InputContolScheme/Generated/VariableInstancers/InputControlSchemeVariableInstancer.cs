using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable Instancer of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `AtomVariableInstancer&lt;InputControlSchemeVariable, InputControlSchemePair, UnityEngine.InputSystem.InputControlScheme, InputControlSchemeEvent, InputControlSchemePairEvent, InputControlSchemeInputControlSchemeFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/InputControlScheme Variable Instancer")]
    public class InputControlSchemeVariableInstancer : AtomVariableInstancer<
        InputControlSchemeVariable,
        InputControlSchemePair,
        UnityEngine.InputSystem.InputControlScheme,
        InputControlSchemeEvent,
        InputControlSchemePairEvent,
        InputControlSchemeInputControlSchemeFunction>
    { }
}
