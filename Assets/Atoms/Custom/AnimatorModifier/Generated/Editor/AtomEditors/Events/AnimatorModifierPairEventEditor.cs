#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `AnimatorModifierPair`. Inherits from `AtomEventEditor&lt;AnimatorModifierPair, AnimatorModifierPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(AnimatorModifierPairEvent))]
    public sealed class AnimatorModifierPairEventEditor : AtomEventEditor<AnimatorModifierPair, AnimatorModifierPairEvent> { }
}
#endif
