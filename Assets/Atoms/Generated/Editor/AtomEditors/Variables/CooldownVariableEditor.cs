using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `Cooldown`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(CooldownVariable))]
    public sealed class CooldownVariableEditor : AtomVariableEditor<Cooldown, CooldownPair> { }
}
