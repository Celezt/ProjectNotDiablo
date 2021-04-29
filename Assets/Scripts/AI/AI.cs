using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    //
    [Header("AI Stats")]

    public float health;
    public float maxHealth;

    [Space(10)]

    public float baseSpeed = 5;
    public float baseAcceleration = 5;
    public float chargeRange;

    [Header("Equiped Weapon")]
    public GameObject selectedWeapon;
    private float cooldownTimer;


    //Hidden Varibles
    bool atDestination;
    bool isCharging = false;
    bool enemyVisible;
    public bool PlayerInRange = false;

    PatrolArea selectedPatrolArea;
    NavMeshAgent agent;

    private FieldOfView fieldOfView;

    private Vector3 destination;
    public GameObject player;

    private enum AiState { Idle, Fleeing, InCombat, Patrolling };
    private AiState currentState;

    //floats
    float weaponRange;
    float distanceToPlayer;
    float distanceToDestination;
    float ChargeSpeed;
    float chargeAcceleration;

    AnimatorBehaviour animator;
    //Weapon Stats


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponentInChildren<AnimatorBehaviour>();
        agent = GetComponent<NavMeshAgent>();
        fieldOfView = gameObject.GetComponentInChildren<FieldOfView>();
        agent.acceleration = baseAcceleration;
        chargeAcceleration = baseAcceleration * 2;
        agent.speed = baseSpeed;
        ChargeSpeed = baseSpeed * 2;
        atDestination = true;
    }
    
    void OnEnable()
    {
        FindPatrolArea();
        if (selectedWeapon.GetComponent<Ranged>() != null)
        {
            weaponRange = selectedWeapon.GetComponent<Ranged>().range;
        }
        if (selectedWeapon.GetComponent<Melee>() != null)
        {
            weaponRange = selectedWeapon.GetComponent<Melee>().range;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (fieldOfView.visableTargets.Count != 0)
        {
            player = fieldOfView.visableTargets[0];
            currentState = AiState.InCombat;
            
        }
        if (player != null)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            Alert();
            currentState = AiState.InCombat;
        }
        if (destination != null && currentState == AiState.InCombat)
        {
            MoveCloser();
        }
        //if (agent.velocity == Vector3.zero)
        //{
        //    animator.SmoothLocalMotion = agent.velocity.normalized;
        //}
        //else if (agent.velocity != Vector3.zero)
        //{
        //}
        animator.SmoothLocalMotion = agent.velocity.normalized;
        //Debug.Log(agent.velocity.normalized);

        distanceToDestination = agent.remainingDistance;
        if (currentState == AiState.Patrolling)
        {
            Patrol();
            if (distanceToDestination <= 0.5)
            {
                atDestination = true;
            }
        }
        else if (currentState == AiState.InCombat && weaponRange < distanceToPlayer)
        {
            MoveCloser();
        }
        if (PlayerInRange)
        {
            Attack();
        }
    }


    void MoveCloser()
    {
        if (chargeRange >= distanceToPlayer)
        {
            agent.acceleration = chargeAcceleration;
            agent.speed = ChargeSpeed;
            isCharging = true; 
        }
        else
        {
            isCharging = false;
            agent.acceleration = baseAcceleration;
            agent.speed = baseSpeed;
        }

        if (weaponRange - 1 > distanceToPlayer)
        {
            destination = gameObject.transform.position;
            if (player != null)
            {
                gameObject.transform.LookAt(player.transform);
                PlayerInRange = true;
            }
        }
        else if (weaponRange + 1 < distanceToPlayer)
        {
            destination = player.transform.position;
            PlayerInRange = false;
        }
        agent.SetDestination(destination);
    }

    void Attack()
    {
        if (selectedWeapon.GetComponent<Ranged>() != null)
        {
            selectedWeapon.GetComponent<Ranged>().Attack(player.transform.position);
        }
        if (selectedWeapon.GetComponent<Melee>() != null)
        {
            selectedWeapon.GetComponent<Melee>().Attack(transform);
        }
    }

    void Flee()
    {
        //Implement Flee behaviour
    }

    void Patrol()
    {
        if (atDestination == true && currentState == AiState.Patrolling)
        {
            atDestination = false;
            int posIndex = UnityEngine.Random.Range(0, selectedPatrolArea.patrolPoints.Count);
            destination = selectedPatrolArea.patrolPoints[posIndex].position;
            agent.SetDestination(destination);
        }
    }

    void FindPatrolArea()
    {
        Collider[] patrolAreas = Physics.OverlapSphere(transform.position, 50, LayerMask.GetMask("PatrolAreas"));
        foreach (Collider patrolArea in patrolAreas)
        {
            Vector3 directionToTarget = patrolArea.gameObject.transform.position - gameObject.transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            float closestDistanceSqr = Mathf.Infinity;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                selectedPatrolArea = patrolArea.gameObject.GetComponent<PatrolArea>();
            }
        }
    }
    void Alert()
    {
        Collider[] listeners = Physics.OverlapSphere(transform.position, 40, LayerMask.GetMask("AI"));
        foreach (Collider listner in listeners)
        {
            listner.gameObject.GetComponent<AI>().player = player;
        }
    }
    void Idle()
    {
        //Do idle Animation
    }
}
