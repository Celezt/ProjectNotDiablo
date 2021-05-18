#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `InputControlSchemePair`. Inherits from `AtomDrawer&lt;InputControlSchemePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(InputControlSchemePairEvent))]
    public class InputControlSchemePairEventDrawer : AtomDrawer<InputControlSchemePairEvent> { }
}
#endif
