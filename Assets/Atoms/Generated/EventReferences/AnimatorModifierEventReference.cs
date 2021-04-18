using System;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `AnimatorModifier`. Inherits from `AtomEventReference&lt;AnimatorModifier, AnimatorModifierVariable, AnimatorModifierEvent, AnimatorModifierVariableInstancer, AnimatorModifierEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class AnimatorModifierEventReference : AtomEventReference<
        AnimatorModifier,
        AnimatorModifierVariable,
        AnimatorModifierEvent,
        AnimatorModifierVariableInstancer,
        AnimatorModifierEventInstancer>, IGetEvent 
    { }
}
