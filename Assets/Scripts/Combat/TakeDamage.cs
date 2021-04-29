using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
public class TakeDamage : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject hitObject;

    [SerializeField]
    FloatVariable variable;

    public void ReciveDamage(float damage)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //player Hit logic
            variable.Value -= damage;
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
