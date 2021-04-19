#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `Cooldown`. Inherits from `AtomDrawer&lt;CooldownVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(CooldownVariable))]
    public class CooldownVariableDrawer : VariableDrawer<CooldownVariable> { }
}
#endif
