using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `MovementType`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(MovementTypeVariable))]
    public sealed class MovementTypeVariableEditor : AtomVariableEditor<MovementType, MovementTypePair> { }
}
