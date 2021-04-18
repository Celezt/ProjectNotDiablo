#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `MovementType`. Inherits from `AtomEventEditor&lt;MovementType, MovementTypeEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(MovementTypeEvent))]
    public sealed class MovementTypeEventEditor : AtomEventEditor<MovementType, MovementTypeEvent> { }
}
#endif
