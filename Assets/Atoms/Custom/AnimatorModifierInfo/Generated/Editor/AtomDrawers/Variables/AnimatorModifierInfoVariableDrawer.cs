#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `AnimatorModifierInfo`. Inherits from `AtomDrawer&lt;AnimatorModifierInfoVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(AnimatorModifierInfoVariable))]
    public class AnimatorModifierInfoVariableDrawer : VariableDrawer<AnimatorModifierInfoVariable> { }
}
#endif
