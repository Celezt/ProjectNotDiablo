using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : WeaponsAndSpells
{
    public float speed;
    public float AreaOfEffekt;

    List<GameObject> targetsAffected = new List<GameObject>();
    public Vector3 destination;
    float distanceTraveled;

    [SerializeField]
    private Spell SelectedSpell;
    [SerializeField]
    private GameObject HitPartileEffects;
    [SerializeField]
    private GameObject travelPartileEffect;

    public Vector3 originPoint;

    //void Update()
    //{
    //    // calculate distance to move
    //    float step = SelectedSpell.speed * Time.deltaTime;
    //    transform.position = Vector3.MoveTowards(transform.position, destination, step);

    //    //Mesure Distance
    //    float distanceToTarget = Vector3.Distance(transform.position, destination);
    //    //If near destination or maxrange the spell will detonate.
    //    if (distanceToTarget <= 0 || distanceTraveled >= SelectedSpell.range)
    //    {
    //        Activate();
    //    }
    //}

    void Activate()
    {
        FindTargets(SelectedSpell.AreaOfEffekt);

        //GameObject projectileFired = (GameObject)Instantiate(HitPartileEffects, transform.position, transform.rotation);

        foreach (GameObject item in targetsAffected)
        {
            if (item.GetComponent<TakeDamage>() != null)
            {
                item.GetComponent<TakeDamage>().ReciveDamage(SelectedSpell.damage);
            }
        }
    }

    void FindTargets(float areaOfEffect)
    {
        targetsAffected.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, areaOfEffect, SelectedSpell.targetLayer);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            GameObject target = targetsInViewRadius[i].gameObject;
            Transform targetTansform = target.GetComponent<Transform>();
            Vector3 directionToTarget = (targetTansform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, targetTansform.position);

            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, SelectedSpell.ignoreLayer))
            {
                targetsAffected.Add(target);
                //directLineOfSight = true;
            }
        }
    }
}
