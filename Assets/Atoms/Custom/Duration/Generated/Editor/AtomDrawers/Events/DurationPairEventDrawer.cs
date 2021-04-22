#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `DurationPair`. Inherits from `AtomDrawer&lt;DurationPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(DurationPairEvent))]
    public class DurationPairEventDrawer : AtomDrawer<DurationPairEvent> { }
}
#endif
