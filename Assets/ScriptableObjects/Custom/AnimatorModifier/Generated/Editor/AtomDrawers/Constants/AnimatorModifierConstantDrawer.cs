#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `AnimatorModifier`. Inherits from `AtomDrawer&lt;AnimatorModifierConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(AnimatorModifierConstant))]
    public class AnimatorModifierConstantDrawer : VariableDrawer<AnimatorModifierConstant> { }
}
#endif
