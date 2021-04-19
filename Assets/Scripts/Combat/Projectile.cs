using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float speed;
    public float radius;
    public float distanceTraveled;
    public float range;

    private List<GameObject> targetsAffected = new List<GameObject>();
    private Vector3 originPoint;

    private Vector3 destination;
    public void SetVaribles(float _damage, float _speed, float _radius, Vector3 _destination, float _range)
    {
        damage = _damage;
        speed = _speed;
        radius = _radius;
        destination = _destination;
        range = _range;
    }

    LayerMask targetLayer;
    LayerMask ignoreLayer;


    void Start()
    {
        targetLayer = LayerMask.GetMask("Damageble");
        ignoreLayer = LayerMask.GetMask("Enviorment");
    }

    private void Awake()
    {
        originPoint = transform.position;
    }

    void Update()
    {
        // calculate distance to move
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination, step);

        //Mesure Distance
        float distanceToTarget = Vector3.Distance(transform.position, destination);
        float distanceTraveled = Vector3.Distance(originPoint, transform.position);
        //If near destination or maxrange the spell will detonate.
        if (distanceToTarget <= 0 || distanceTraveled >= range)
        {
            Activate();
        }
    }

    void Activate()
    {
        FindTargets(radius);

        foreach (GameObject item in targetsAffected)
        {
            Debug.Log(item);
            if (item.GetComponent<TakeDamage>() != null)
            {
                item.GetComponent<TakeDamage>().ReciveDamage(damage);
            }
        }
        Destroy(gameObject);
    }

    void FindTargets(float areaOfEffect)
    {
        targetsAffected.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, areaOfEffect, targetLayer);

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
    }
}
