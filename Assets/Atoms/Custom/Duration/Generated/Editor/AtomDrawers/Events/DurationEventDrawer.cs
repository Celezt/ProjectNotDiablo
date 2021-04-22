#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `Duration`. Inherits from `AtomDrawer&lt;DurationEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(DurationEvent))]
    public class DurationEventDrawer : AtomDrawer<DurationEvent> { }
}
#endif
