#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `MovementType`. Inherits from `AtomDrawer&lt;MovementTypeValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(MovementTypeValueList))]
    public class MovementTypeValueListDrawer : AtomDrawer<MovementTypeValueList> { }
}
#endif
