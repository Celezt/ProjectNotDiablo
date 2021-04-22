using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Value List of type `Duration`. Inherits from `AtomValueList&lt;Duration, DurationEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/Duration", fileName = "DurationValueList")]
    public sealed class DurationValueList : AtomValueList<Duration, DurationEvent> { }
}
