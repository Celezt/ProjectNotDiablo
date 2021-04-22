using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Set variable value Action of type `AnimatorModifier`. Inherits from `SetVariableValue&lt;AnimatorModifier, AnimatorModifierPair, AnimatorModifierVariable, AnimatorModifierConstant, AnimatorModifierReference, AnimatorModifierEvent, AnimatorModifierPairEvent, AnimatorModifierVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/AnimatorModifier", fileName = "SetAnimatorModifierVariableValue")]
    public sealed class SetAnimatorModifierVariableValue : SetVariableValue<
        AnimatorModifier,
        AnimatorModifierPair,
        AnimatorModifierVariable,
        AnimatorModifierConstant,
        AnimatorModifierReference,
        AnimatorModifierEvent,
        AnimatorModifierPairEvent,
        AnimatorModifierAnimatorModifierFunction,
        AnimatorModifierVariableInstancer>
    { }
}
