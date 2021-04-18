using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable Instancer of type `Cooldown`. Inherits from `AtomVariableInstancer&lt;CooldownVariable, CooldownPair, Cooldown, CooldownEvent, CooldownPairEvent, CooldownCooldownFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/Cooldown Variable Instancer")]
    public class CooldownVariableInstancer : AtomVariableInstancer<
        CooldownVariable,
        CooldownPair,
        Cooldown,
        CooldownEvent,
        CooldownPairEvent,
        CooldownCooldownFunction>
    { }
}
