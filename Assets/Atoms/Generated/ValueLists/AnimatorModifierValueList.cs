using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Value List of type `AnimatorModifier`. Inherits from `AtomValueList&lt;AnimatorModifier, AnimatorModifierEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/AnimatorModifier", fileName = "AnimatorModifierValueList")]
    public sealed class AnimatorModifierValueList : AtomValueList<AnimatorModifier, AnimatorModifierEvent> { }
}
