using System;
using UnityEngine;
namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// IPair of type `&lt;Cooldown&gt;`. Inherits from `IPair&lt;Cooldown&gt;`.
    /// </summary>
    [Serializable]
    public struct CooldownPair : IPair<Cooldown>
    {
        public Cooldown Item1 { get => _item1; set => _item1 = value; }
        public Cooldown Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private Cooldown _item1;
        [SerializeField]
        private Cooldown _item2;

        public void Deconstruct(out Cooldown item1, out Cooldown item2) { item1 = Item1; item2 = Item2; }
    }
}