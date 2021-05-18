using System;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Reference of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `AtomReference&lt;UnityEngine.InputSystem.InputControlScheme, InputControlSchemePair, InputControlSchemeConstant, InputControlSchemeVariable, InputControlSchemeEvent, InputControlSchemePairEvent, InputControlSchemeInputControlSchemeFunction, InputControlSchemeVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class InputControlSchemeReference : AtomReference<
        UnityEngine.InputSystem.InputControlScheme,
        InputControlSchemePair,
        InputControlSchemeConstant,
        InputControlSchemeVariable,
        InputControlSchemeEvent,
        InputControlSchemePairEvent,
        InputControlSchemeInputControlSchemeFunction,
        InputControlSchemeVariableInstancer>, IEquatable<InputControlSchemeReference>
    {
        public InputControlSchemeReference() : base() { }
        public InputControlSchemeReference(UnityEngine.InputSystem.InputControlScheme value) : base(value) { }
        public bool Equals(InputControlSchemeReference other) { return base.Equals(other); }
        protected override bool ValueEquals(UnityEngine.InputSystem.InputControlScheme other)
        {
            throw new NotImplementedException();
        }
    }
}
