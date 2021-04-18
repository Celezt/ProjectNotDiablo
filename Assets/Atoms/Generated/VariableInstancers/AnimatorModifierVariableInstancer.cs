using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable Instancer of type `AnimatorModifier`. Inherits from `AtomVariableInstancer&lt;AnimatorModifierVariable, AnimatorModifierPair, AnimatorModifier, AnimatorModifierEvent, AnimatorModifierPairEvent, AnimatorModifierAnimatorModifierFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/AnimatorModifier Variable Instancer")]
    public class AnimatorModifierVariableInstancer : AtomVariableInstancer<
        AnimatorModifierVariable,
        AnimatorModifierPair,
        AnimatorModifier,
        AnimatorModifierEvent,
        AnimatorModifierPairEvent,
        AnimatorModifierAnimatorModifierFunction>
    { }
}
