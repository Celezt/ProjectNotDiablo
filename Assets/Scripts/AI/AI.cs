using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public FieldOfView fieldOfView;
    bool enemyVisible;
    public float baseSpeed = 5;
    float ChargeSpeed;
    public float baseAcceleration = 5;
    float chargeAcceleration;
    NavMeshAgent agent;
    bool isCharging;
    public Vector3 destination;
    public GameObject player;
    public enum AiState { Idle, Fleeing, InCombat, Patrolling };
    public AiState currentState;
    public float weaponRange;
    public float distanceToPlayer;
    public float distanceToDestination;
    public float chargeRange;

    public float wanderRadius;
    public float wanderTimer;
    private float timer;

    public PatrolArea selectedPatrolArea;

    bool atDestination;


    // Start is called before the first frame update
    void Start()
    {
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

        if (weaponRange > distanceToPlayer)
        {
            destination = gameObject.transform.position;
            if (player != null)
            {
                gameObject.transform.LookAt(player.transform);
            }
        }
        else if (weaponRange < distanceToPlayer)
        {
            destination = player.transform.position;

        }
        agent.SetDestination(destination);
    }

    void Attack()
    {
        //Weapon Attack
        //GetComponentInChildren<MeleeAttack>(Attack).(GetcomponentInChild<Weapon>)
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
