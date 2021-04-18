using System;
using UnityEngine.Events;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// None generic Unity Event of type `MovementType`. Inherits from `UnityEvent&lt;MovementType&gt;`.
    /// </summary>
    [Serializable]
    public sealed class MovementTypeUnityEvent : UnityEvent<MovementType> { }
}
