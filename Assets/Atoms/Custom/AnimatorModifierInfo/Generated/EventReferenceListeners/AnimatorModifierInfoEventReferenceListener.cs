using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `AnimatorModifierInfo`. Inherits from `AtomEventReferenceListener&lt;AnimatorModifierInfo, AnimatorModifierInfoEvent, AnimatorModifierInfoEventReference, AnimatorModifierInfoUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/AnimatorModifierInfo Event Reference Listener")]
    public sealed class AnimatorModifierInfoEventReferenceListener : AtomEventReferenceListener<
        AnimatorModifierInfo,
        AnimatorModifierInfoEvent,
        AnimatorModifierInfoEventReference,
        AnimatorModifierInfoUnityEvent>
    { }
}
