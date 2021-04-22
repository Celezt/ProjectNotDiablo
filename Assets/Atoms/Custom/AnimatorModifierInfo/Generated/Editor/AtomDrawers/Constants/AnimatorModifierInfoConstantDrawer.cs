#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `AnimatorModifierInfo`. Inherits from `AtomDrawer&lt;AnimatorModifierInfoConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(AnimatorModifierInfoConstant))]
    public class AnimatorModifierInfoConstantDrawer : VariableDrawer<AnimatorModifierInfoConstant> { }
}
#endif
