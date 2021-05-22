using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField]AI boss;
    Animator animator;
    bool open;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && boss.dead == true)
        {
            //End Game
        }
    }
    private void Update()
    {
        if (boss.dead == true)
        {
            animator.Play("OpenGateWay");
        }
    }
}
