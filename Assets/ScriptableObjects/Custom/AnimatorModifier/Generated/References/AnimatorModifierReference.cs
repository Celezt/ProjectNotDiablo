using System;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Reference of type `AnimatorModifier`. Inherits from `AtomReference&lt;AnimatorModifier, AnimatorModifierPair, AnimatorModifierConstant, AnimatorModifierVariable, AnimatorModifierEvent, AnimatorModifierPairEvent, AnimatorModifierAnimatorModifierFunction, AnimatorModifierVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class AnimatorModifierReference : AtomReference<
        AnimatorModifier,
        AnimatorModifierPair,
        AnimatorModifierConstant,
        AnimatorModifierVariable,
        AnimatorModifierEvent,
        AnimatorModifierPairEvent,
        AnimatorModifierAnimatorModifierFunction,
        AnimatorModifierVariableInstancer>, IEquatable<AnimatorModifierReference>
    {
        public AnimatorModifierReference() : base() { }
        public AnimatorModifierReference(AnimatorModifier value) : base(value) { }
        public bool Equals(AnimatorModifierReference other) { return base.Equals(other); }
        protected override bool ValueEquals(AnimatorModifier other)
        {
            throw new NotImplementedException();
        }
    }
}
