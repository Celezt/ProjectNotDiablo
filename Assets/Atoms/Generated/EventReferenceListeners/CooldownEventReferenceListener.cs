using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `Cooldown`. Inherits from `AtomEventReferenceListener&lt;Cooldown, CooldownEvent, CooldownEventReference, CooldownUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/Cooldown Event Reference Listener")]
    public sealed class CooldownEventReferenceListener : AtomEventReferenceListener<
        Cooldown,
        CooldownEvent,
        CooldownEventReference,
        CooldownUnityEvent>
    { }
}
