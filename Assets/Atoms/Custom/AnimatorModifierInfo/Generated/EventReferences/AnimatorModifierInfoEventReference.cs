using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `AnimatorModifierInfo`. Inherits from `AtomEventReference&lt;AnimatorModifierInfo, AnimatorModifierInfoVariable, AnimatorModifierInfoEvent, AnimatorModifierInfoVariableInstancer, AnimatorModifierInfoEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class AnimatorModifierInfoEventReference : AtomEventReference<
        AnimatorModifierInfo,
        AnimatorModifierInfoVariable,
        AnimatorModifierInfoEvent,
        AnimatorModifierInfoVariableInstancer,
        AnimatorModifierInfoEventInstancer>, IGetEvent 
    { }
}
