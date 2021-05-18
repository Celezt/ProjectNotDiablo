#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `InputControlSchemePair`. Inherits from `AtomEventEditor&lt;InputControlSchemePair, InputControlSchemePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(InputControlSchemePairEvent))]
    public sealed class InputControlSchemePairEventEditor : AtomEventEditor<InputControlSchemePair, InputControlSchemePairEvent> { }
}
#endif
