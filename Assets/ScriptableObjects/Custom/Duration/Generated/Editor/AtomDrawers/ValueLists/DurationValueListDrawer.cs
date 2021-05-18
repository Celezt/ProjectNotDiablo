#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `Duration`. Inherits from `AtomDrawer&lt;DurationValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(DurationValueList))]
    public class DurationValueListDrawer : AtomDrawer<DurationValueList> { }
}
#endif
