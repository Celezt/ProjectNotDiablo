using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `MovementTypePair`. Inherits from `AtomEventReferenceListener&lt;MovementTypePair, MovementTypePairEvent, MovementTypePairEventReference, MovementTypePairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/MovementTypePair Event Reference Listener")]
    public sealed class MovementTypePairEventReferenceListener : AtomEventReferenceListener<
        MovementTypePair,
        MovementTypePairEvent,
        MovementTypePairEventReference,
        MovementTypePairUnityEvent>
    { }
}
