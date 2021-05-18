using System;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Reference of type `Duration`. Inherits from `AtomReference&lt;Duration, DurationPair, DurationConstant, DurationVariable, DurationEvent, DurationPairEvent, DurationDurationFunction, DurationVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class DurationReference : AtomReference<
        Duration,
        DurationPair,
        DurationConstant,
        DurationVariable,
        DurationEvent,
        DurationPairEvent,
        DurationDurationFunction,
        DurationVariableInstancer>, IEquatable<DurationReference>
    {
        public DurationReference() : base() { }
        public DurationReference(Duration value) : base(value) { }
        public bool Equals(DurationReference other) { return base.Equals(other); }
        protected override bool ValueEquals(Duration other)
        {
            throw new NotImplementedException();
        }
    }
}
