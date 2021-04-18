using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `AnimatorModifierInfoPair`. Inherits from `AtomEventReferenceListener&lt;AnimatorModifierInfoPair, AnimatorModifierInfoPairEvent, AnimatorModifierInfoPairEventReference, AnimatorModifierInfoPairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/AnimatorModifierInfoPair Event Reference Listener")]
    public sealed class AnimatorModifierInfoPairEventReferenceListener : AtomEventReferenceListener<
        AnimatorModifierInfoPair,
        AnimatorModifierInfoPairEvent,
        AnimatorModifierInfoPairEventReference,
        AnimatorModifierInfoPairUnityEvent>
    { }
}
