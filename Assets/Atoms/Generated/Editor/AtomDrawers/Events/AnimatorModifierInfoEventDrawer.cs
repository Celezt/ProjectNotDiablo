#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `AnimatorModifierInfo`. Inherits from `AtomDrawer&lt;AnimatorModifierInfoEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(AnimatorModifierInfoEvent))]
    public class AnimatorModifierInfoEventDrawer : AtomDrawer<AnimatorModifierInfoEvent> { }
}
#endif
