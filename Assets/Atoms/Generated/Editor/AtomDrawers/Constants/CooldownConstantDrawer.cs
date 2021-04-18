#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `Cooldown`. Inherits from `AtomDrawer&lt;CooldownConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(CooldownConstant))]
    public class CooldownConstantDrawer : VariableDrawer<CooldownConstant> { }
}
#endif
