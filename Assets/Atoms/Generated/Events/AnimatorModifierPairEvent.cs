using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `AnimatorModifierPair`. Inherits from `AtomEvent&lt;AnimatorModifierPair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/AnimatorModifierPair", fileName = "AnimatorModifierPairEvent")]
    public sealed class AnimatorModifierPairEvent : AtomEvent<AnimatorModifierPair>
    {
    }
}
