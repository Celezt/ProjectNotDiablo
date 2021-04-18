#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `AnimatorModifierInfo`. Inherits from `AtomEventEditor&lt;AnimatorModifierInfo, AnimatorModifierInfoEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(AnimatorModifierInfoEvent))]
    public sealed class AnimatorModifierInfoEventEditor : AtomEventEditor<AnimatorModifierInfo, AnimatorModifierInfoEvent> { }
}
#endif
