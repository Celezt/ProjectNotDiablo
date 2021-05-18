using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `AnimatorModifier`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(AnimatorModifierVariable))]
    public sealed class AnimatorModifierVariableEditor : AtomVariableEditor<AnimatorModifier, AnimatorModifierPair> { }
}
