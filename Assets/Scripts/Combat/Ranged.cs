using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float damage;
    public float cooldown;
    public float range;
    public float areaOfEffectRadius;
    public float speed;

    [Header("Additional Stats")]
    public bool cleve;
    public bool modifier;
    public Vector3 pointOfOrigin;

    public GameObject projetileModel;

    private float distanceTraveled;

    private List<GameObject> hitableTargets = new List<GameObject>();

    private float cooldownTimer;

    void Start()
    {
        cooldownTimer = 0;
    }
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public void Attack(Vector3 destination)
    {
        if (cooldownTimer <= 0)
        {
            if (pointOfOrigin == new Vector3(0, 0, 0))
            {
                GameObject projectileFired = projetileModel;
                projectileFired = (GameObject)Instantiate(projetileModel, transform.position, transform.rotation);
                projectileFired.GetComponent<Projectile>().SetVaribles(damage, speed, areaOfEffectRadius, destination, range);
            }
            else if (pointOfOrigin != new Vector3(0, 0, 0))
            {
                projetileModel.GetComponent<Projectile>().SetVaribles(damage, speed, areaOfEffectRadius, destination, range);
                destination += pointOfOrigin;
                projetileModel = (GameObject)Instantiate(projetileModel, destination, transform.rotation);
            }
            cooldownTimer = cooldown;
        }
    }
}
