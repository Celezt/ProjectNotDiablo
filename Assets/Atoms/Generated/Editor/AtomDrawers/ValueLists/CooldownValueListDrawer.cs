#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `Cooldown`. Inherits from `AtomDrawer&lt;CooldownValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(CooldownValueList))]
    public class CooldownValueListDrawer : AtomDrawer<CooldownValueList> { }
}
#endif
