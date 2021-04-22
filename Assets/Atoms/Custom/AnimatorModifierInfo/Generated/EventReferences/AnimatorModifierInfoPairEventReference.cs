using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `AnimatorModifierInfoPair`. Inherits from `AtomEventReference&lt;AnimatorModifierInfoPair, AnimatorModifierInfoVariable, AnimatorModifierInfoPairEvent, AnimatorModifierInfoVariableInstancer, AnimatorModifierInfoPairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class AnimatorModifierInfoPairEventReference : AtomEventReference<
        AnimatorModifierInfoPair,
        AnimatorModifierInfoVariable,
        AnimatorModifierInfoPairEvent,
        AnimatorModifierInfoVariableInstancer,
        AnimatorModifierInfoPairEventInstancer>, IGetEvent 
    { }
}
