using System;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Reference of type `Cooldown`. Inherits from `AtomReference&lt;Cooldown, CooldownPair, CooldownConstant, CooldownVariable, CooldownEvent, CooldownPairEvent, CooldownCooldownFunction, CooldownVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class CooldownReference : AtomReference<
        Cooldown,
        CooldownPair,
        CooldownConstant,
        CooldownVariable,
        CooldownEvent,
        CooldownPairEvent,
        CooldownCooldownFunction,
        CooldownVariableInstancer>, IEquatable<CooldownReference>
    {
        public CooldownReference() : base() { }
        public CooldownReference(Cooldown value) : base(value) { }
        public bool Equals(CooldownReference other) { return base.Equals(other); }
        protected override bool ValueEquals(Cooldown other)
        {
            throw new NotImplementedException();
        }
    }
}
