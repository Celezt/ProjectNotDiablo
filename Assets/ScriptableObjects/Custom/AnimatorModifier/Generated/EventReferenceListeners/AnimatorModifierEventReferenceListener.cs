using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `AnimatorModifier`. Inherits from `AtomEventReferenceListener&lt;AnimatorModifier, AnimatorModifierEvent, AnimatorModifierEventReference, AnimatorModifierUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/AnimatorModifier Event Reference Listener")]
    public sealed class AnimatorModifierEventReferenceListener : AtomEventReferenceListener<
        AnimatorModifier,
        AnimatorModifierEvent,
        AnimatorModifierEventReference,
        AnimatorModifierUnityEvent>
    { }
}
