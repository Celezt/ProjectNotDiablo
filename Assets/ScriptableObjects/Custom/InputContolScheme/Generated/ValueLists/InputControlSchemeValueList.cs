using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Value List of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `AtomValueList&lt;UnityEngine.InputSystem.InputControlScheme, InputControlSchemeEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/InputControlScheme", fileName = "InputControlSchemeValueList")]
    public sealed class InputControlSchemeValueList : AtomValueList<UnityEngine.InputSystem.InputControlScheme, InputControlSchemeEvent> { }
}
