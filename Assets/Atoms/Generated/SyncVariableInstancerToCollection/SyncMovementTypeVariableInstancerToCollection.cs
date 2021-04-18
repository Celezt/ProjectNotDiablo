using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Adds Variable Instancer's Variable of type MovementType to a Collection or List on OnEnable and removes it on OnDestroy. 
    /// </summary>
    [AddComponentMenu("Unity Atoms/Sync Variable Instancer to Collection/Sync MovementType Variable Instancer to Collection")]
    [EditorIcon("atom-icon-delicate")]
    public class SyncMovementTypeVariableInstancerToCollection : SyncVariableInstancerToCollection<MovementType, MovementTypeVariable, MovementTypeVariableInstancer> { }
}
