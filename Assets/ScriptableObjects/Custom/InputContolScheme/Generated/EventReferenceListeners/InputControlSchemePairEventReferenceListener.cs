using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `InputControlSchemePair`. Inherits from `AtomEventReferenceListener&lt;InputControlSchemePair, InputControlSchemePairEvent, InputControlSchemePairEventReference, InputControlSchemePairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/InputControlSchemePair Event Reference Listener")]
    public sealed class InputControlSchemePairEventReferenceListener : AtomEventReferenceListener<
        InputControlSchemePair,
        InputControlSchemePairEvent,
        InputControlSchemePairEventReference,
        InputControlSchemePairUnityEvent>
    { }
}
