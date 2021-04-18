using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `CooldownPair`. Inherits from `AtomEventInstancer&lt;CooldownPair, CooldownPairEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/CooldownPair Event Instancer")]
    public class CooldownPairEventInstancer : AtomEventInstancer<CooldownPair, CooldownPairEvent> { }
}
