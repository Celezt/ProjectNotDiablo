using System;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Reference of type `AnimatorModifierInfo`. Inherits from `AtomReference&lt;AnimatorModifierInfo, AnimatorModifierInfoPair, AnimatorModifierInfoConstant, AnimatorModifierInfoVariable, AnimatorModifierInfoEvent, AnimatorModifierInfoPairEvent, AnimatorModifierInfoAnimatorModifierInfoFunction, AnimatorModifierInfoVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class AnimatorModifierInfoReference : AtomReference<
        AnimatorModifierInfo,
        AnimatorModifierInfoPair,
        AnimatorModifierInfoConstant,
        AnimatorModifierInfoVariable,
        AnimatorModifierInfoEvent,
        AnimatorModifierInfoPairEvent,
        AnimatorModifierInfoAnimatorModifierInfoFunction,
        AnimatorModifierInfoVariableInstancer>, IEquatable<AnimatorModifierInfoReference>
    {
        public AnimatorModifierInfoReference() : base() { }
        public AnimatorModifierInfoReference(AnimatorModifierInfo value) : base(value) { }
        public bool Equals(AnimatorModifierInfoReference other) { return base.Equals(other); }
        protected override bool ValueEquals(AnimatorModifierInfo other)
        {
            throw new NotImplementedException();
        }
    }
}
