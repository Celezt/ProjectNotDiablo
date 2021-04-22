#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `Duration`. Inherits from `AtomEventEditor&lt;Duration, DurationEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(DurationEvent))]
    public sealed class DurationEventEditor : AtomEventEditor<Duration, DurationEvent> { }
}
#endif
