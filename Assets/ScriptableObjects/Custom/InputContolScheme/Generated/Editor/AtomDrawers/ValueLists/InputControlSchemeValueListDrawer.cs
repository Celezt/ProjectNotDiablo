#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `UnityEngine.InputSystem.InputControlScheme`. Inherits from `AtomDrawer&lt;InputControlSchemeValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(InputControlSchemeValueList))]
    public class InputControlSchemeValueListDrawer : AtomDrawer<InputControlSchemeValueList> { }
}
#endif
