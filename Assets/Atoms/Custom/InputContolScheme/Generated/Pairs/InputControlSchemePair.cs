using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// IPair of type `&lt;UnityEngine.InputSystem.InputControlScheme&gt;`. Inherits from `IPair&lt;UnityEngine.InputSystem.InputControlScheme&gt;`.
    /// </summary>
    [Serializable]
    public struct InputControlSchemePair : IPair<UnityEngine.InputSystem.InputControlScheme>
    {
        public UnityEngine.InputSystem.InputControlScheme Item1 { get => _item1; set => _item1 = value; }
        public UnityEngine.InputSystem.InputControlScheme Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private UnityEngine.InputSystem.InputControlScheme _item1;
        [SerializeField]
        private UnityEngine.InputSystem.InputControlScheme _item2;

        public void Deconstruct(out UnityEngine.InputSystem.InputControlScheme item1, out UnityEngine.InputSystem.InputControlScheme item2) { item1 = Item1; item2 = Item2; }
    }
}