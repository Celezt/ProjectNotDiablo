#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `AtomDrawer&lt;InputControlSchemeEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(InputControlSchemeEvent))]
    public class InputControlSchemeEventDrawer : AtomDrawer<InputControlSchemeEvent> { }
}
#endif
