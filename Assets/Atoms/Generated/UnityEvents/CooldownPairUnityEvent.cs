using System;
using UnityEngine.Events;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// None generic Unity Event of type `CooldownPair`. Inherits from `UnityEvent&lt;CooldownPair&gt;`.
    /// </summary>
    [Serializable]
    public sealed class CooldownPairUnityEvent : UnityEvent<CooldownPair> { }
}
