using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `CooldownPair`. Inherits from `AtomEventReferenceListener&lt;CooldownPair, CooldownPairEvent, CooldownPairEventReference, CooldownPairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/CooldownPair Event Reference Listener")]
    public sealed class CooldownPairEventReferenceListener : AtomEventReferenceListener<
        CooldownPair,
        CooldownPairEvent,
        CooldownPairEventReference,
        CooldownPairUnityEvent>
    { }
}
