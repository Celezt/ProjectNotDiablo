using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `MovementTypePair`. Inherits from `AtomEventInstancer&lt;MovementTypePair, MovementTypePairEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/MovementTypePair Event Instancer")]
    public class MovementTypePairEventInstancer : AtomEventInstancer<MovementTypePair, MovementTypePairEvent> { }
}
