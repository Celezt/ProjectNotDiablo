using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `AnimatorModifier`. Inherits from `AtomEventInstancer&lt;AnimatorModifier, AnimatorModifierEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/AnimatorModifier Event Instancer")]
    public class AnimatorModifierEventInstancer : AtomEventInstancer<AnimatorModifier, AnimatorModifierEvent> { }
}
