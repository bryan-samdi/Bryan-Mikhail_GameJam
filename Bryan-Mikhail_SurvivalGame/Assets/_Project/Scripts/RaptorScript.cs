using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RaptorScript : MonoBehaviour
{
    public Transform player;
    public float health;
    Animator bearAnim;

    // Patroling/Wandering
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Chasing
    public float sightRange, chaseRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public float wanderSpeed = 1.5f;
    public float chaseSpeed = 3.5f;
    public float wanderTime = 5.0f;
    private float wanderTimer;

    // Attacking
    public float timeBetweenAttacks;
    private float attackTimer;
    public int attackDamage = 10;

    // Movement and Rotation
    public float rotationSpeed = 5f;

    // Chase timer and cooldown
    public float chaseDuration = 20f;
    private float chaseTimer;
    public float chaseCooldown = 15f; // New cooldown for chasing
    private float chaseCooldownTimer;
    private bool isChaseCooldown;

    private NavMeshAgent agent;
    private enum State { Wander, Chase, Attack }
    private State currentState;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        bearAnim = GetComponent<Animator>();
        wanderTimer = wanderTime;
        attackTimer = 0f;
        chaseTimer = 0f;
        chaseCooldownTimer = 0f;
        isChaseCooldown = false;
        SwitchState(State.Wander);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        attackTimer -= Time.deltaTime;

        playerInSightRange = distanceToPlayer <= sightRange;
        playerInAttackRange = distanceToPlayer <= attackRange;

        if (isChaseCooldown)
        {
            chaseCooldownTimer -= Time.deltaTime;
            if (chaseCooldownTimer <= 0f)
            {
                isChaseCooldown = false;
            }
        }

        switch (currentState)
        {
            case State.Wander:
                Wander();
                if (playerInSightRange && !playerInAttackRange && !isChaseCooldown)
                {
                    SwitchState(State.Chase);
                }
                break;

            case State.Chase:
                Chase();
                if (playerInAttackRange)
                {
                    SwitchState(State.Attack);
                }
                else if (!playerInSightRange || chaseTimer >= chaseDuration)
                {
                    SwitchState(State.Wander);
                    StartChaseCooldown();
                }
                break;

            case State.Attack:
                Attack();
                if (!playerInAttackRange)
                {
                    SwitchState(State.Chase);
                }
                break;
        }
    }

    private void Wander()
    {
        bearAnim.SetTrigger("RaptorWalk");
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderTime)
        {
            Vector3 newPos = RandomNavSphere(transform.position, walkPointRange, -1);
            agent.SetDestination(newPos);
            wanderTimer = 0f;
        }

        agent.speed = wanderSpeed;
    }

    private void Chase()
    {
        bearAnim.SetTrigger("RaptorChase");
        agent.SetDestination(player.position);
        agent.speed = chaseSpeed;
        SmoothRotateTowards(player.position);

        chaseTimer += Time.deltaTime;
        if (chaseTimer >= chaseDuration)
        {
            SwitchState(State.Wander);
            StartChaseCooldown();
            return;
        }

        if (attackTimer <= 0f && playerInAttackRange)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            attackTimer = timeBetweenAttacks;
        }
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);
        SmoothRotateTowards(player.position);

        if (attackTimer <= 0f && playerInAttackRange)
        {
            bearAnim.SetTrigger("RaptorAttack");
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            attackTimer = timeBetweenAttacks;
        }
        else if (attackTimer <= 0f && !playerInAttackRange)
        {
            SwitchState(State.Chase);
        }
    }

    private void SwitchState(State newState)
    {
        currentState = newState;

        if (newState == State.Wander)
        {
            wanderTimer = wanderTime;
            chaseTimer = 0f;
        }
        else if (newState == State.Chase)
        {
            chaseTimer = 0f;
        }
    }

    private void StartChaseCooldown()
    {
        isChaseCooldown = true;
        chaseCooldownTimer = chaseCooldown;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) DestroyEnemy();
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void SmoothRotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
