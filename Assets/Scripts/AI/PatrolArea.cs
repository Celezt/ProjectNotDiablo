using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolArea : MonoBehaviour
{
    public List<Transform> patrolPoints = new List<Transform>();


    void Start()
    {
        foreach (Transform child in transform)
        {
            patrolPoints.Add(child);
        }
    }
}
