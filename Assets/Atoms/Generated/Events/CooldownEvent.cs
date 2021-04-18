using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `Cooldown`. Inherits from `AtomEvent&lt;Cooldown&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/Cooldown", fileName = "CooldownEvent")]
    public sealed class CooldownEvent : AtomEvent<Cooldown>
    {
    }
}
