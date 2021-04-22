using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `Duration`. Inherits from `AtomEventReferenceListener&lt;Duration, DurationEvent, DurationEventReference, DurationUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/Duration Event Reference Listener")]
    public sealed class DurationEventReferenceListener : AtomEventReferenceListener<
        Duration,
        DurationEvent,
        DurationEventReference,
        DurationUnityEvent>
    { }
}
