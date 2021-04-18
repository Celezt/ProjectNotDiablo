using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsAndSpells : MonoBehaviour
{
    public float damage;
    public float modifier;
    public float cooldown;
    public float range;
    

    [HideInInspector]
    public LayerMask targetLayer;
    [HideInInspector]
    public LayerMask ignoreLayer;

    // Start is called before the first frame update
    void Start()
    {
        targetLayer = LayerMask.GetMask("Damageble");
        ignoreLayer = LayerMask.GetMask("Enviorment");
    }
}
