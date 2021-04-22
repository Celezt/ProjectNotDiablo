using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `AnimatorModifierInfo`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(AnimatorModifierInfoVariable))]
    public sealed class AnimatorModifierInfoVariableEditor : AtomVariableEditor<AnimatorModifierInfo, AnimatorModifierInfoPair> { }
}
