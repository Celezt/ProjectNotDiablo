#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `Duration`. Inherits from `AtomDrawer&lt;DurationVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(DurationVariable))]
    public class DurationVariableDrawer : VariableDrawer<DurationVariable> { }
}
#endif
