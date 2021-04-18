using UnityEngine;
using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable of type `Cooldown`. Inherits from `AtomVariable&lt;Cooldown, CooldownPair, CooldownEvent, CooldownPairEvent, CooldownCooldownFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/Cooldown", fileName = "CooldownVariable")]
    public sealed class CooldownVariable : AtomVariable<Cooldown, CooldownPair, CooldownEvent, CooldownPairEvent, CooldownCooldownFunction>
    {
        protected override bool ValueEquals(Cooldown other)
        {
            throw new NotImplementedException();
        }
    }
}
