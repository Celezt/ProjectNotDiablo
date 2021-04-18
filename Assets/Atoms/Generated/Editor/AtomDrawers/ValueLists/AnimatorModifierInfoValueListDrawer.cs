#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `AnimatorModifierInfo`. Inherits from `AtomDrawer&lt;AnimatorModifierInfoValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(AnimatorModifierInfoValueList))]
    public class AnimatorModifierInfoValueListDrawer : AtomDrawer<AnimatorModifierInfoValueList> { }
}
#endif
