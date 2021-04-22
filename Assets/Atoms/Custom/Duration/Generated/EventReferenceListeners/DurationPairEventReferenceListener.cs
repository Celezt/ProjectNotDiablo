using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `DurationPair`. Inherits from `AtomEventReferenceListener&lt;DurationPair, DurationPairEvent, DurationPairEventReference, DurationPairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/DurationPair Event Reference Listener")]
    public sealed class DurationPairEventReferenceListener : AtomEventReferenceListener<
        DurationPair,
        DurationPairEvent,
        DurationPairEventReference,
        DurationPairUnityEvent>
    { }
}
