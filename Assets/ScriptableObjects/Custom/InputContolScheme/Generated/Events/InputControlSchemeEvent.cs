using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `AtomEvent&lt;UnityEngine.InputSystem.InputControlScheme&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/InputControlScheme", fileName = "InputControlSchemeEvent")]
    public sealed class InputControlSchemeEvent : AtomEvent<UnityEngine.InputSystem.InputControlScheme>
    {
    }
}
