#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `Cooldown`. Inherits from `AtomDrawer&lt;CooldownEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(CooldownEvent))]
    public class CooldownEventDrawer : AtomDrawer<CooldownEvent> { }
}
#endif
