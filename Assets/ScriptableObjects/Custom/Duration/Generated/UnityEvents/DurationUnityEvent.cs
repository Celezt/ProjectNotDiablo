using System;
using UnityEngine.Events;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// None generic Unity Event of type `Duration`. Inherits from `UnityEvent&lt;Duration&gt;`.
    /// </summary>
    [Serializable]
    public sealed class DurationUnityEvent : UnityEvent<Duration> { }
}
