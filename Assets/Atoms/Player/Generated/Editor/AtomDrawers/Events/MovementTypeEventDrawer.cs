#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `MovementType`. Inherits from `AtomDrawer&lt;MovementTypeEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(MovementTypeEvent))]
    public class MovementTypeEventDrawer : AtomDrawer<MovementTypeEvent> { }
}
#endif
