using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Value List of type `AnimatorModifierInfo`. Inherits from `AtomValueList&lt;AnimatorModifierInfo, AnimatorModifierInfoEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/AnimatorModifierInfo", fileName = "AnimatorModifierInfoValueList")]
    public sealed class AnimatorModifierInfoValueList : AtomValueList<AnimatorModifierInfo, AnimatorModifierInfoEvent> { }
}
