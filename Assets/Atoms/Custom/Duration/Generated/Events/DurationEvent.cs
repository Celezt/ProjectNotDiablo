using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `Duration`. Inherits from `AtomEvent&lt;Duration&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/Duration", fileName = "DurationEvent")]
    public sealed class DurationEvent : AtomEvent<Duration>
    {
    }
}
