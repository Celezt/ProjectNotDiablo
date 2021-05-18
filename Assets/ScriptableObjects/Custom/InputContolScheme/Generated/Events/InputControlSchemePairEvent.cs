using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `InputControlSchemePair`. Inherits from `AtomEvent&lt;InputControlSchemePair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/InputControlSchemePair", fileName = "InputControlSchemePairEvent")]
    public sealed class InputControlSchemePairEvent : AtomEvent<InputControlSchemePair>
    {
    }
}
