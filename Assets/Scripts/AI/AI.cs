using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityAtoms.BaseAtoms;

public class AI : MonoBehaviour
{
    //
    [Header("AI Stats")]

    public FloatReference health;
    public FloatReference maxHealth;

    [Space(10)]

    public float baseSpeed = 5;
    public float baseAcceleration = 5;
    public float chargeRange;

    [Header("Equiped Weapon")]
    public GameObject selectedWeapon;
    private float cooldownTimer;

    //Hidden Varibles
    bool atDestination;
    bool hasAlerted = false;
    bool isCharging = false;
    bool enemyVisible;
    public bool PlayerInRange = false;
    public bool dead;
    public bool Stunned;

    Collider collider;

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

    AnimatorBehaviour animatorBehaviour;

    [SerializeField]
    AnimationClip attackAnimation;
    [SerializeField]
    AnimationClip dyingAnimation;

    AudioSource audio;
    [SerializeField]
    AudioClip deathClip;
    [SerializeField]
    AudioClip spottedClip;
    public AudioClip hitSoundClip;
    //Weapon Stats

    LayerMask selfLayer;

    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        animatorBehaviour = gameObject.GetComponentInChildren<AnimatorBehaviour>();
        agent = GetComponent<NavMeshAgent>();
        fieldOfView = gameObject.GetComponentInChildren<FieldOfView>();
        agent.acceleration = baseAcceleration;
        chargeAcceleration = baseAcceleration * 2;
        agent.speed = baseSpeed;
        ChargeSpeed = baseSpeed * 2;
        atDestination = true;
        dead = false;
        selfLayer = LayerMask.NameToLayer("AI");
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
    public void CallStunned()
    {
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(2);
        Stunned = false;
    }
    // Update is called once per frame
    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("AI")) ;
        {
            destination = new Vector3(1, 0, 1) + transform.position;
            agent.destination = destination;
        }
    }
    void Update()
    {

        if (health <= 0 && dead != true)
        {
            Death();
            enabled = false;
        }
        if (dead == true || Stunned == true)
        {
            return;
        }

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
        animatorBehaviour.SmoothLocalMotion = agent.velocity.normalized;
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

    void Death()
    {
        audio.clip = deathClip;
        audio.Play();
        animatorBehaviour.RaiseDying();
        animatorBehaviour.EnableCustomAnimation = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject, 2f);
        dead = true;
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
            selectedWeapon.GetComponent<Melee>().Attack(transform, gameObject.GetComponent<Collider>());

            if (animatorBehaviour.IsAnimationModifierRunning == false)
            {
                animatorBehaviour.OnAnimationModifierRaised(new AnimatorModifier(attackAnimation));
            }
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
        if (hasAlerted == false)
        {
            audio.clip = spottedClip;
            audio.Play();
            Collider[] listeners = Physics.OverlapSphere(transform.position, 20, LayerMask.GetMask("AI"));
            foreach (Collider listner in listeners)
            {
                listner.gameObject.GetComponent<AI>().player = player;
            }
            hasAlerted = true;
        }

    }
    void Idle()
    {
        //Do idle Animation
    }
}
