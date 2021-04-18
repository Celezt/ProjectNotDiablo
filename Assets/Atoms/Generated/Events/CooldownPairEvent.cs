using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `CooldownPair`. Inherits from `AtomEvent&lt;CooldownPair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/CooldownPair", fileName = "CooldownPairEvent")]
    public sealed class CooldownPairEvent : AtomEvent<CooldownPair>
    {
    }
}
