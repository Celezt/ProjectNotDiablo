using System;
using UnityEngine;
namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// IPair of type `&lt;AnimatorModifier&gt;`. Inherits from `IPair&lt;AnimatorModifier&gt;`.
    /// </summary>
    [Serializable]
    public struct AnimatorModifierPair : IPair<AnimatorModifier>
    {
        public AnimatorModifier Item1 { get => _item1; set => _item1 = value; }
        public AnimatorModifier Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private AnimatorModifier _item1;
        [SerializeField]
        private AnimatorModifier _item2;

        public void Deconstruct(out AnimatorModifier item1, out AnimatorModifier item2) { item1 = Item1; item2 = Item2; }
    }
}