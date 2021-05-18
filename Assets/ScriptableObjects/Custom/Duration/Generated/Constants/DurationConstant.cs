using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Constant of type `Duration`. Inherits from `AtomBaseVariable&lt;Duration&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-teal")]
    [CreateAssetMenu(menuName = "Unity Atoms/Constants/Duration", fileName = "DurationConstant")]
    public sealed class DurationConstant : AtomBaseVariable<Duration> { }
}
