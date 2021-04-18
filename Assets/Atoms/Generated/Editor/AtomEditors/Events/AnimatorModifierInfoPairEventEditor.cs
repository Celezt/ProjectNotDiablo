#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `AnimatorModifierInfoPair`. Inherits from `AtomEventEditor&lt;AnimatorModifierInfoPair, AnimatorModifierInfoPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(AnimatorModifierInfoPairEvent))]
    public sealed class AnimatorModifierInfoPairEventEditor : AtomEventEditor<AnimatorModifierInfoPair, AnimatorModifierInfoPairEvent> { }
}
#endif
