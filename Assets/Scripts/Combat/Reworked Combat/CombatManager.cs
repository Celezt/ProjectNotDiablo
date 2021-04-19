using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    protected List<GameObject> targetsAffected = new List<GameObject>();

    // Start is called before the first frame update


    protected void TransferDamage(float damage, float modifier)
    {
        foreach (GameObject item in targetsAffected)
        {
            if (item.GetComponent<TakeDamage>() != null)
            {
                item.GetComponent<TakeDamage>().ReciveDamage(damage);
            }
        }
    }
}
