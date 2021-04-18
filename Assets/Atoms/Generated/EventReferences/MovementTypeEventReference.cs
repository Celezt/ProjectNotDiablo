using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `MovementType`. Inherits from `AtomEventReference&lt;MovementType, MovementTypeVariable, MovementTypeEvent, MovementTypeVariableInstancer, MovementTypeEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class MovementTypeEventReference : AtomEventReference<
        MovementType,
        MovementTypeVariable,
        MovementTypeEvent,
        MovementTypeVariableInstancer,
        MovementTypeEventInstancer>, IGetEvent 
    { }
}
