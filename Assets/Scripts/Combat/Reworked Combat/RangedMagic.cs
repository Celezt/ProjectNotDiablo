using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMagic : CombatManager
{
    float speed;
    Vector3 destination;
    float distanceTraveled;
    float range;
    bool AreaOfEffect;

    void Attack()
    {

    }

    //private void Update()
    //{
    //    float step = speed * Time.deltaTime;
    //    transform.position = Vector3.MoveTowards(transform.position, destination, step);

    //    //Mesure Distance
    //    float distanceToTarget = Vector3.Distance(transform.position, destination);
    //    //If near destination or maxrange the spell will detonate.
    //    if (distanceToTarget <= 0 || distanceTraveled >= range)
    //    {
    //        targetsAffected = gameObject.GetComponent<RayCastAreaTransfer>().FindTargets();
    //    }
    //}
}
