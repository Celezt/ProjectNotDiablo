using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `AnimatorModifierPair`. Inherits from `AtomEventReference&lt;AnimatorModifierPair, AnimatorModifierVariable, AnimatorModifierPairEvent, AnimatorModifierVariableInstancer, AnimatorModifierPairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class AnimatorModifierPairEventReference : AtomEventReference<
        AnimatorModifierPair,
        AnimatorModifierVariable,
        AnimatorModifierPairEvent,
        AnimatorModifierVariableInstancer,
        AnimatorModifierPairEventInstancer>, IGetEvent 
    { }
}
