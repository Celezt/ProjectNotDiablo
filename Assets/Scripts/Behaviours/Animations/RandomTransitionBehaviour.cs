using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityAtoms.BaseAtoms;

public class RandomTransitionBehaviour : StateMachineBehaviour
{
    public string ParameterName;
    public List<int> Transitions;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int index = Random.Range(0, Transitions.Count);
        animator.SetInteger(ParameterName, Transitions[index]);
    }
}
