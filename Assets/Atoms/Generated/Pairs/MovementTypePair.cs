using System;
using UnityEngine;
namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// IPair of type `&lt;MovementType&gt;`. Inherits from `IPair&lt;MovementType&gt;`.
    /// </summary>
    [Serializable]
    public struct MovementTypePair : IPair<MovementType>
    {
        public MovementType Item1 { get => _item1; set => _item1 = value; }
        public MovementType Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private MovementType _item1;
        [SerializeField]
        private MovementType _item2;

        public void Deconstruct(out MovementType item1, out MovementType item2) { item1 = Item1; item2 = Item2; }
    }
}