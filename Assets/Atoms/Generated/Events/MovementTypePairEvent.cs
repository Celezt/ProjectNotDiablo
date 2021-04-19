using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `MovementTypePair`. Inherits from `AtomEvent&lt;MovementTypePair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/MovementTypePair", fileName = "MovementTypePairEvent")]
    public sealed class MovementTypePairEvent : AtomEvent<MovementTypePair>
    {
    }
}
