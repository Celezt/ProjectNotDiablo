using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject hitObject;
    public void ReciveDamage(float damage)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //player Hit logic
            
            Debug.Log("OW-EEE");
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Agent"))
        {
            hitObject.GetComponent<AI>().health -= damage;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("DamagebleObjects"))
        {

        }
    }
}
