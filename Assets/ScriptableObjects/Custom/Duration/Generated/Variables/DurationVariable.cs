using UnityEngine;
using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable of type `Duration`. Inherits from `AtomVariable&lt;Duration, DurationPair, DurationEvent, DurationPairEvent, DurationDurationFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/Duration", fileName = "DurationVariable")]
    public sealed class DurationVariable : AtomVariable<Duration, DurationPair, DurationEvent, DurationPairEvent, DurationDurationFunction>
    {
        protected override bool ValueEquals(Duration other) => other.ID == Value.ID;
    }
}
