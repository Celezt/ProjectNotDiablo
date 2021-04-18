#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `CooldownPair`. Inherits from `AtomEventEditor&lt;CooldownPair, CooldownPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(CooldownPairEvent))]
    public sealed class CooldownPairEventEditor : AtomEventEditor<CooldownPair, CooldownPairEvent> { }
}
#endif
