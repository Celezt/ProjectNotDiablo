#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `MovementTypePair`. Inherits from `AtomEventEditor&lt;MovementTypePair, MovementTypePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(MovementTypePairEvent))]
    public sealed class MovementTypePairEventEditor : AtomEventEditor<MovementTypePair, MovementTypePairEvent> { }
}
#endif