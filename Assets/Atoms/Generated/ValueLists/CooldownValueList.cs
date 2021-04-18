using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Value List of type `Cooldown`. Inherits from `AtomValueList&lt;Cooldown, CooldownEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/Cooldown", fileName = "CooldownValueList")]
    public sealed class CooldownValueList : AtomValueList<Cooldown, CooldownEvent> { }
}
