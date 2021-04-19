#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `AnimatorModifier`. Inherits from `AtomDrawer&lt;AnimatorModifierEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(AnimatorModifierEvent))]
    public class AnimatorModifierEventDrawer : AtomDrawer<AnimatorModifierEvent> { }
}
#endif
