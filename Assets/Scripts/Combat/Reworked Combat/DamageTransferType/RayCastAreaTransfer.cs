using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastAreaTransfer : TransferType
{
    public override List<GameObject> FindTargets(float radius)
    {
        List<GameObject> targetsAffected = new List<GameObject>();
        targetsAffected.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, radius, targetLayer);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            GameObject target = targetsInViewRadius[i].gameObject;
            Transform targetTansform = target.GetComponent<Transform>();
            Vector3 directionToTarget = (targetTansform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, targetTansform.position);

            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ignoreLayer))
            {
                targetsAffected.Add(target);
                //directLineOfSight = true;
            }
        }
        return targetsAffected;
    }
}
