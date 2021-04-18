using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `MovementType`. Inherits from `AtomEventReferenceListener&lt;MovementType, MovementTypeEvent, MovementTypeEventReference, MovementTypeUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/MovementType Event Reference Listener")]
    public sealed class MovementTypeEventReferenceListener : AtomEventReferenceListener<
        MovementType,
        MovementTypeEvent,
        MovementTypeEventReference,
        MovementTypeUnityEvent>
    { }
}
