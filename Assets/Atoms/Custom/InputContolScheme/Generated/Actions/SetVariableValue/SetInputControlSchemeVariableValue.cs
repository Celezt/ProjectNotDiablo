using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Set variable value Action of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `SetVariableValue&lt;UnityEngine.InputSystem.InputControlScheme, InputControlSchemePair, InputControlSchemeVariable, InputControlSchemeConstant, InputControlSchemeReference, InputControlSchemeEvent, InputControlSchemePairEvent, InputControlSchemeVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/InputControlScheme", fileName = "SetInputControlSchemeVariableValue")]
    public sealed class SetInputControlSchemeVariableValue : SetVariableValue<
        UnityEngine.InputSystem.InputControlScheme,
        InputControlSchemePair,
        InputControlSchemeVariable,
        InputControlSchemeConstant,
        InputControlSchemeReference,
        InputControlSchemeEvent,
        InputControlSchemePairEvent,
        InputControlSchemeInputControlSchemeFunction,
        InputControlSchemeVariableInstancer>
    { }
}
