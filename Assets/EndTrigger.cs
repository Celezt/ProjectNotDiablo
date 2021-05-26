using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    AI boss;
    Animator animator;
    bool open;
    

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (boss != null)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") && open == true)
            {
                //End Game
                other.gameObject.GetComponent<TakeDamage>().ReciveDamage(1000);
            }
        }
    }
    private void Update()
    {
        if (boss != null)
        {
            if (boss.dead == true)
            {
                open = true;
                animator.Play("OpenGateWay");
            }
        }
        if (boss == null)
        {
            StartCoroutine(FindBoss());
        }
    }

    IEnumerator FindBoss()
    {
        yield return new WaitForSecondsRealtime(10);
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<AI>();

    }
}
