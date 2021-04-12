using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask ignoreLayer;
    public LayerMask targetLayer;

    public List<GameObject> visableTargets = new List<GameObject>();

    void Start()
    {
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }
    private void Update()
    {
        FindVisableTargets();
    }
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            
        }
    }
    void FindVisableTargets()
    {
        visableTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetLayer);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            GameObject target = targetsInViewRadius[i].gameObject;
            Transform targetTansform = target.GetComponent<Transform>();
            Vector3 directionToTarget = (targetTansform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetTansform.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ignoreLayer))
                {
                    visableTargets.Add(target);
                    //directLineOfSight = true;
                }
            }
        }
    }

    public Vector3 DirectionFromAngle(float angeleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angeleDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angeleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angeleDegrees * Mathf.Deg2Rad));
    }
}
