using System;
using UnityEngine.Events;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// None generic Unity Event of type `Cooldown`. Inherits from `UnityEvent&lt;Cooldown&gt;`.
    /// </summary>
    [Serializable]
    public sealed class CooldownUnityEvent : UnityEvent<Cooldown> { }
}
