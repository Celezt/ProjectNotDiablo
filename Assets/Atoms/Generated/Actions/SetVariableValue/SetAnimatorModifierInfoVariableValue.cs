using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Set variable value Action of type `AnimatorModifierInfo`. Inherits from `SetVariableValue&lt;AnimatorModifierInfo, AnimatorModifierInfoPair, AnimatorModifierInfoVariable, AnimatorModifierInfoConstant, AnimatorModifierInfoReference, AnimatorModifierInfoEvent, AnimatorModifierInfoPairEvent, AnimatorModifierInfoVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/AnimatorModifierInfo", fileName = "SetAnimatorModifierInfoVariableValue")]
    public sealed class SetAnimatorModifierInfoVariableValue : SetVariableValue<
        AnimatorModifierInfo,
        AnimatorModifierInfoPair,
        AnimatorModifierInfoVariable,
        AnimatorModifierInfoConstant,
        AnimatorModifierInfoReference,
        AnimatorModifierInfoEvent,
        AnimatorModifierInfoPairEvent,
        AnimatorModifierInfoAnimatorModifierInfoFunction,
        AnimatorModifierInfoVariableInstancer>
    { }
}
