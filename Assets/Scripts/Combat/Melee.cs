using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{

    [Header("Weapon Stats")]
    public float damage;
    public float cooldown;
    public float range;
    public float angle;

    [Header("Additional Stats")]
    public bool cleve;
    public bool modifier;

    public List<GameObject> hitableTargets = new List<GameObject>();

    private float cooldownTimer;

    LayerMask targetLayer;
    LayerMask ignoreLayer;

    private Transform originPos;

    void Start()
    {
        targetLayer = LayerMask.GetMask("Damageble", "Player", "AI");
        ignoreLayer = LayerMask.GetMask("Ignore Raycast");
        cooldownTimer = 0;
    }
    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public void Attack(Transform pos)
    {
        originPos = pos;
        if (cooldownTimer <= 0)
        {
            if (cleve)
            {
                FindTargets(180, range);
                foreach (GameObject item in hitableTargets)
                {
                    if (hitableTargets.Count != 0)
                    {
                        if (item.GetComponent<TakeDamage>() != null)
                        {
                            item.GetComponent<TakeDamage>().ReciveDamage(damage);
                        }
                    }
                }
            }
            else
            {
                FindTargets(angle, range);
                if (hitableTargets.Count != 0)
                {
                    if (hitableTargets[0].GetComponent<TakeDamage>() != null)
                    {
                        hitableTargets[0].GetComponent<TakeDamage>().ReciveDamage(damage);
                    }
                }
            }
            cooldownTimer = cooldown;
        }
    }

    void FindTargets(float angle, float range)
    {
        hitableTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(originPos.position, range, targetLayer);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            GameObject target = targetsInViewRadius[i].gameObject;
            Transform targetTansform = target.GetComponent<Transform>();
            Vector3 directionToTarget = (targetTansform.position - originPos.position).normalized;

            float distanceToTarget = Vector3.Distance(originPos.position, targetTansform.position);

            if (!Physics.Raycast(originPos.position, directionToTarget, distanceToTarget, ignoreLayer))
            {
                hitableTargets.Add(target);
            }
            Debug.DrawLine(transform.position, targetTansform.position, Color.white, 2.5f);
        }
    }

}
