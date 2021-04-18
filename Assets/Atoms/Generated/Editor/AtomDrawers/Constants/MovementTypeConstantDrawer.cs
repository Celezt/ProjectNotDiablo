#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `MovementType`. Inherits from `AtomDrawer&lt;MovementTypeConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(MovementTypeConstant))]
    public class MovementTypeConstantDrawer : VariableDrawer<MovementTypeConstant> { }
}
#endif
