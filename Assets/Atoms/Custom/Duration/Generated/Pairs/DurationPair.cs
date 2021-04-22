using System;
using UnityEngine;
namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// IPair of type `&lt;Duration&gt;`. Inherits from `IPair&lt;Duration&gt;`.
    /// </summary>
    [Serializable]
    public struct DurationPair : IPair<Duration>
    {
        public Duration Item1 { get => _item1; set => _item1 = value; }
        public Duration Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private Duration _item1;
        [SerializeField]
        private Duration _item2;

        public void Deconstruct(out Duration item1, out Duration item2) { item1 = Item1; item2 = Item2; }
    }
}