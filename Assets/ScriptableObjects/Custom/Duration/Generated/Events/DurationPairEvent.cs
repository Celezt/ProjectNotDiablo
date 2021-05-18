using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `DurationPair`. Inherits from `AtomEvent&lt;DurationPair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/DurationPair", fileName = "DurationPairEvent")]
    public sealed class DurationPairEvent : AtomEvent<DurationPair>
    {
    }
}
