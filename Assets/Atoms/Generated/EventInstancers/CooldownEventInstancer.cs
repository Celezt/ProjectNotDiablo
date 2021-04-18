using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `Cooldown`. Inherits from `AtomEventInstancer&lt;Cooldown, CooldownEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/Cooldown Event Instancer")]
    public class CooldownEventInstancer : AtomEventInstancer<Cooldown, CooldownEvent> { }
}
