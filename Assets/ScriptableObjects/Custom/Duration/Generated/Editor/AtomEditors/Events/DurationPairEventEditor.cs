#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `DurationPair`. Inherits from `AtomEventEditor&lt;DurationPair, DurationPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(DurationPairEvent))]
    public sealed class DurationPairEventEditor : AtomEventEditor<DurationPair, DurationPairEvent> { }
}
#endif
