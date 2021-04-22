using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `Duration`. Inherits from `AtomEventInstancer&lt;Duration, DurationEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/Duration Event Instancer")]
    public class DurationEventInstancer : AtomEventInstancer<Duration, DurationEvent> { }
}
