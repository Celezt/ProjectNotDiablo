using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Value List of type `MovementType`. Inherits from `AtomValueList&lt;MovementType, MovementTypeEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/MovementType", fileName = "MovementTypeValueList")]
    public sealed class MovementTypeValueList : AtomValueList<MovementType, MovementTypeEvent> { }
}
