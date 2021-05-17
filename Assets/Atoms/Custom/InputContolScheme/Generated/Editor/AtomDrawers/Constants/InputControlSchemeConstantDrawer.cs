#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `AtomDrawer&lt;InputControlSchemeConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(InputControlSchemeConstant))]
    public class InputControlSchemeConstantDrawer : VariableDrawer<InputControlSchemeConstant> { }
}
#endif
