using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `AnimatorModifierInfoPair`. Inherits from `AtomEvent&lt;AnimatorModifierInfoPair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/AnimatorModifierInfoPair", fileName = "AnimatorModifierInfoPairEvent")]
    public sealed class AnimatorModifierInfoPairEvent : AtomEvent<AnimatorModifierInfoPair>
    {
    }
}
