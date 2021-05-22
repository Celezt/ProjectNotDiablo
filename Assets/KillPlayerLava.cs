using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerLava : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Kill Player
            gameObject.GetComponent<TakeDamage>().ReciveDamage(1000);
        }
    }
}
