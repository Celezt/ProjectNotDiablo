using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransferType : MonoBehaviour
{
    [HideInInspector]
    public LayerMask targetLayer;
    [HideInInspector]
    public LayerMask ignoreLayer;

    void Start()
    {
        targetLayer = LayerMask.GetMask("Damageble");
        ignoreLayer = LayerMask.GetMask("Enviorment");
    }

    public abstract List<GameObject> FindTargets(float radius);
}
