using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `AnimatorModifier`. Inherits from `AtomEvent&lt;AnimatorModifier&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/AnimatorModifier", fileName = "AnimatorModifierEvent")]
    public sealed class AnimatorModifierEvent : AtomEvent<AnimatorModifier>
    {
    }
}
