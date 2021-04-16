using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `MovementType`. Inherits from `AtomEventInstancer&lt;MovementType, MovementTypeEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/MovementType Event Instancer")]
    public class MovementTypeEventInstancer : AtomEventInstancer<MovementType, MovementTypeEvent> { }
}
