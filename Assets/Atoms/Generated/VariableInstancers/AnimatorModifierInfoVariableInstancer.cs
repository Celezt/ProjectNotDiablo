using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable Instancer of type `AnimatorModifierInfo`. Inherits from `AtomVariableInstancer&lt;AnimatorModifierInfoVariable, AnimatorModifierInfoPair, AnimatorModifierInfo, AnimatorModifierInfoEvent, AnimatorModifierInfoPairEvent, AnimatorModifierInfoAnimatorModifierInfoFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/AnimatorModifierInfo Variable Instancer")]
    public class AnimatorModifierInfoVariableInstancer : AtomVariableInstancer<
        AnimatorModifierInfoVariable,
        AnimatorModifierInfoPair,
        AnimatorModifierInfo,
        AnimatorModifierInfoEvent,
        AnimatorModifierInfoPairEvent,
        AnimatorModifierInfoAnimatorModifierInfoFunction>
    { }
}
