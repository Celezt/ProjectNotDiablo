#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `Cooldown`. Inherits from `AtomEventEditor&lt;Cooldown, CooldownEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(CooldownEvent))]
    public sealed class CooldownEventEditor : AtomEventEditor<Cooldown, CooldownEvent> { }
}
#endif
