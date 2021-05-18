#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `AnimatorModifier`. Inherits from `AtomDrawer&lt;AnimatorModifierVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(AnimatorModifierVariable))]
    public class AnimatorModifierVariableDrawer : VariableDrawer<AnimatorModifierVariable> { }
}
#endif
