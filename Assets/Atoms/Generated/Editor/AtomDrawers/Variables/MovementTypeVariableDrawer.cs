#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `MovementType`. Inherits from `AtomDrawer&lt;MovementTypeVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(MovementTypeVariable))]
    public class MovementTypeVariableDrawer : VariableDrawer<MovementTypeVariable> { }
}
#endif
