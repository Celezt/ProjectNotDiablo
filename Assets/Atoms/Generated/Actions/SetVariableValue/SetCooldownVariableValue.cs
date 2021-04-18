using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Set variable value Action of type `Cooldown`. Inherits from `SetVariableValue&lt;Cooldown, CooldownPair, CooldownVariable, CooldownConstant, CooldownReference, CooldownEvent, CooldownPairEvent, CooldownVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/Cooldown", fileName = "SetCooldownVariableValue")]
    public sealed class SetCooldownVariableValue : SetVariableValue<
        Cooldown,
        CooldownPair,
        CooldownVariable,
        CooldownConstant,
        CooldownReference,
        CooldownEvent,
        CooldownPairEvent,
        CooldownCooldownFunction,
        CooldownVariableInstancer>
    { }
}
