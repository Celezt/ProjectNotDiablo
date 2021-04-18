using UnityEngine;
using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable of type `AnimatorModifierInfo`. Inherits from `AtomVariable&lt;AnimatorModifierInfo, AnimatorModifierInfoPair, AnimatorModifierInfoEvent, AnimatorModifierInfoPairEvent, AnimatorModifierInfoAnimatorModifierInfoFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/AnimatorModifierInfo", fileName = "AnimatorModifierInfoVariable")]
    public sealed class AnimatorModifierInfoVariable : AtomVariable<AnimatorModifierInfo, AnimatorModifierInfoPair, AnimatorModifierInfoEvent, AnimatorModifierInfoPairEvent, AnimatorModifierInfoAnimatorModifierInfoFunction>
    {
        protected override bool ValueEquals(AnimatorModifierInfo other)
        {
            throw new NotImplementedException();
        }
    }
}
