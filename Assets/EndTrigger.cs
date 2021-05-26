using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    AI boss;
    Animator animator;
    bool open;


    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        if (boss == null)
        {
            StartCoroutine(FindBoss());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Kills player and ends game
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && open == true)
        {
            other.gameObject.GetComponent<TakeDamage>().ReciveDamage(1000);
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

    }

    IEnumerator FindBoss()
    {
        yield return new WaitForSecondsRealtime(4);
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<AI>();
        Debug.Log(boss);
    }
}
