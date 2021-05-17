#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `AtomEventEditor&lt;UnityEngine.InputSystem.InputControlScheme, InputControlSchemeEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(InputControlSchemeEvent))]
    public sealed class InputControlSchemeEventEditor : AtomEventEditor<UnityEngine.InputSystem.InputControlScheme, InputControlSchemeEvent> { }
}
#endif
