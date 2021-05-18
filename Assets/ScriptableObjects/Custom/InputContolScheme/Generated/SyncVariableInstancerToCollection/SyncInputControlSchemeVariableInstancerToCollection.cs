using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Adds Variable Instancer's Variable of type UnityEngine.InputSystem.InputControlScheme to a Collection or List on OnEnable and removes it on OnDestroy. 
    /// </summary>
    [AddComponentMenu("Unity Atoms/Sync Variable Instancer to Collection/Sync InputControlScheme Variable Instancer to Collection")]
    [EditorIcon("atom-icon-delicate")]
    public class SyncInputControlSchemeVariableInstancerToCollection : SyncVariableInstancerToCollection<UnityEngine.InputSystem.InputControlScheme, InputControlSchemeVariable, InputControlSchemeVariableInstancer> { }
}
