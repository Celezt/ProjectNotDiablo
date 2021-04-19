using System;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Reference of type `MovementType`. Inherits from `AtomReference&lt;MovementType, MovementTypePair, MovementTypeConstant, MovementTypeVariable, MovementTypeEvent, MovementTypePairEvent, MovementTypeMovementTypeFunction, MovementTypeVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class MovementTypeReference : AtomReference<
        MovementType,
        MovementTypePair,
        MovementTypeConstant,
        MovementTypeVariable,
        MovementTypeEvent,
        MovementTypePairEvent,
        MovementTypeMovementTypeFunction,
        MovementTypeVariableInstancer>, IEquatable<MovementTypeReference>
    {
        public MovementTypeReference() : base() { }
        public MovementTypeReference(MovementType value) : base(value) { }
        public bool Equals(MovementTypeReference other) { return base.Equals(other); }
        protected override bool ValueEquals(MovementType other)
        {
            throw new NotImplementedException();
        }
    }
}
