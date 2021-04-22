using System;
using UnityEngine;
namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// IPair of type `&lt;AnimatorModifierInfo&gt;`. Inherits from `IPair&lt;AnimatorModifierInfo&gt;`.
    /// </summary>
    [Serializable]
    public struct AnimatorModifierInfoPair : IPair<AnimatorModifierInfo>
    {
        public AnimatorModifierInfo Item1 { get => _item1; set => _item1 = value; }
        public AnimatorModifierInfo Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private AnimatorModifierInfo _item1;
        [SerializeField]
        private AnimatorModifierInfo _item2;

        public void Deconstruct(out AnimatorModifierInfo item1, out AnimatorModifierInfo item2) { item1 = Item1; item2 = Item2; }
    }
}