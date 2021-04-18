#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `AnimatorModifierInfoPair`. Inherits from `AtomDrawer&lt;AnimatorModifierInfoPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(AnimatorModifierInfoPairEvent))]
    public class AnimatorModifierInfoPairEventDrawer : AtomDrawer<AnimatorModifierInfoPairEvent> { }
}
#endif
