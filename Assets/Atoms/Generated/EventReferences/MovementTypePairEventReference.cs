using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `MovementTypePair`. Inherits from `AtomEventReference&lt;MovementTypePair, MovementTypeVariable, MovementTypePairEvent, MovementTypeVariableInstancer, MovementTypePairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class MovementTypePairEventReference : AtomEventReference<
        MovementTypePair,
        MovementTypeVariable,
        MovementTypePairEvent,
        MovementTypeVariableInstancer,
        MovementTypePairEventInstancer>, IGetEvent 
    { }
}
