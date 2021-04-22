#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `AnimatorModifier`. Inherits from `AtomDrawer&lt;AnimatorModifierValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(AnimatorModifierValueList))]
    public class AnimatorModifierValueListDrawer : AtomDrawer<AnimatorModifierValueList> { }
}
#endif
