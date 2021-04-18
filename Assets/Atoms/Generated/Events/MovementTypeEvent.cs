using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `MovementType`. Inherits from `AtomEvent&lt;MovementType&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/MovementType", fileName = "MovementTypeEvent")]
    public sealed class MovementTypeEvent : AtomEvent<MovementType>
    {
    }
}
