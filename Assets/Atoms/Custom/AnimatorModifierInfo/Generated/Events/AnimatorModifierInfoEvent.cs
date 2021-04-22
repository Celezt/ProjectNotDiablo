using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `AnimatorModifierInfo`. Inherits from `AtomEvent&lt;AnimatorModifierInfo&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/AnimatorModifierInfo", fileName = "AnimatorModifierInfoEvent")]
    public sealed class AnimatorModifierInfoEvent : AtomEvent<AnimatorModifierInfo>
    {
    }
}
