using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `Duration`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(DurationVariable))]
    public sealed class DurationVariableEditor : AtomVariableEditor<Duration, DurationPair> { }
}
