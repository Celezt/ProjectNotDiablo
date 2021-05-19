using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
public class TakeDamage : MonoBehaviour
{
    // Start is called before the first frame update
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
        else if (gameObject.layer == LayerMask.NameToLayer("AI"))
        {
            Debug.Log("CLANG");
            gameObject.GetComponent<AI>().health.Value -= damage;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Damageble"))
        {

        }
    }
}
