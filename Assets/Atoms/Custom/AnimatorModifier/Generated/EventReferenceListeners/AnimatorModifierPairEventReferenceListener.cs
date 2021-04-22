using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `AnimatorModifierPair`. Inherits from `AtomEventReferenceListener&lt;AnimatorModifierPair, AnimatorModifierPairEvent, AnimatorModifierPairEventReference, AnimatorModifierPairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/AnimatorModifierPair Event Reference Listener")]
    public sealed class AnimatorModifierPairEventReferenceListener : AtomEventReferenceListener<
        AnimatorModifierPair,
        AnimatorModifierPairEvent,
        AnimatorModifierPairEventReference,
        AnimatorModifierPairUnityEvent>
    { }
}
