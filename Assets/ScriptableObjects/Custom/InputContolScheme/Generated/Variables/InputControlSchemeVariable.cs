using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `AtomVariable&lt;UnityEngine.InputSystem.InputControlScheme, InputControlSchemePair, InputControlSchemeEvent, InputControlSchemePairEvent, InputControlSchemeInputControlSchemeFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/InputControlScheme", fileName = "InputControlSchemeVariable")]
    public sealed class InputControlSchemeVariable : AtomVariable<UnityEngine.InputSystem.InputControlScheme, InputControlSchemePair, InputControlSchemeEvent, InputControlSchemePairEvent, InputControlSchemeInputControlSchemeFunction>
    {
        protected override bool ValueEquals(UnityEngine.InputSystem.InputControlScheme other) => Value == other;
    }
}
