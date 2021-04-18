#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `AnimatorModifier`. Inherits from `AtomEventEditor&lt;AnimatorModifier, AnimatorModifierEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(AnimatorModifierEvent))]
    public sealed class AnimatorModifierEventEditor : AtomEventEditor<AnimatorModifier, AnimatorModifierEvent> { }
}
#endif
