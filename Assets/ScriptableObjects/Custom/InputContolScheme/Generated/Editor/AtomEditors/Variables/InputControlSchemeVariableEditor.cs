using UnityEditor;
using UnityAtoms.Editor;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(InputControlSchemeVariable))]
    public sealed class InputControlSchemeVariableEditor : AtomVariableEditor<UnityEngine.InputSystem.InputControlScheme, InputControlSchemePair> { }
}
