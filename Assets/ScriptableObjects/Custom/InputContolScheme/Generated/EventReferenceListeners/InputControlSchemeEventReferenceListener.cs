using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `AtomEventReferenceListener&lt;UnityEngine.InputSystem.InputControlScheme, InputControlSchemeEvent, InputControlSchemeEventReference, InputControlSchemeUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/InputControlScheme Event Reference Listener")]
    public sealed class InputControlSchemeEventReferenceListener : AtomEventReferenceListener<
        UnityEngine.InputSystem.InputControlScheme,
        InputControlSchemeEvent,
        InputControlSchemeEventReference,
        InputControlSchemeUnityEvent>
    { }
}
