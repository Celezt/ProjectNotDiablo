using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `InputControlSchemePair`. Inherits from `AtomEventInstancer&lt;InputControlSchemePair, InputControlSchemePairEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/InputControlSchemePair Event Instancer")]
    public class InputControlSchemePairEventInstancer : AtomEventInstancer<InputControlSchemePair, InputControlSchemePairEvent> { }
}
