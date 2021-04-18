using UnityEngine;
using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable of type `AnimatorModifier`. Inherits from `AtomVariable&lt;AnimatorModifier, AnimatorModifierPair, AnimatorModifierEvent, AnimatorModifierPairEvent, AnimatorModifierAnimatorModifierFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/AnimatorModifier", fileName = "AnimatorModifierVariable")]
    public sealed class AnimatorModifierVariable : AtomVariable<AnimatorModifier, AnimatorModifierPair, AnimatorModifierEvent, AnimatorModifierPairEvent, AnimatorModifierAnimatorModifierFunction>
    {
        protected override bool ValueEquals(AnimatorModifier other) => Value.Equals(other);
    }
}
