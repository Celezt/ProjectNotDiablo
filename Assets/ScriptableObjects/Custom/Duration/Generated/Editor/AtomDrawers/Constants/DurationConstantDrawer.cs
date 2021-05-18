#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `Duration`. Inherits from `AtomDrawer&lt;DurationConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(DurationConstant))]
    public class DurationConstantDrawer : VariableDrawer<DurationConstant> { }
}
#endif
