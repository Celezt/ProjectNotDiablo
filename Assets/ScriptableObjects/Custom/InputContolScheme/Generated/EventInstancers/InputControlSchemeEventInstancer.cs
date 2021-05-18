using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `AtomEventInstancer&lt;UnityEngine.InputSystem.InputControlScheme, InputControlSchemeEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/InputControlScheme Event Instancer")]
    public class InputControlSchemeEventInstancer : AtomEventInstancer<UnityEngine.InputSystem.InputControlScheme, InputControlSchemeEvent> { }
}
