using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FoxScript : MonoBehaviour
{
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;
    Animator animalAnim;

    // Patroling/Wandering
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Running away
    public float runAwayRange;
    public float runAwayDistance;
    public bool playerInRunAwayRange;
    public float wanderSpeed = 1.5f;
    public float runAwaySpeed = 5.0f;
    public float wanderTime = 5.0f;
    private float wanderTimer;

    // Movement and Rotation
    public float rotationSpeed = 5f;

    private NavMeshAgent agent;
    private enum State { Wander, RunAway }
    private State currentState;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animalAnim = GetComponent<Animator>();
        wanderTimer = wanderTime;
        SwitchState(State.Wander);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        playerInRunAwayRange = distanceToPlayer <= runAwayRange;

        switch (currentState)
        {
            case State.Wander:
                Wander();
                if (playerInRunAwayRange)
                {
                    SwitchState(State.RunAway);
                }
                break;

            case State.RunAway:
                RunAway();
                if (!playerInRunAwayRange && agent.remainingDistance <= agent.stoppingDistance)
                {
                    SwitchState(State.Wander);
                }
                break;
        }
    }

    private void Wander()
    {
        animalAnim.SetTrigger("FoxWalk");
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderTime)
        {
            Vector3 newPos = RandomNavSphere(transform.position, walkPointRange, -1);
            agent.SetDestination(newPos);
            wanderTimer = 0f;
        }

        agent.speed = wanderSpeed;
    }

    private void RunAway()
    {
        animalAnim.SetTrigger("FoxRun");

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 runDirection = transform.position - player.position;
            Vector3 newPos = RandomNavSphere(transform.position + runDirection.normalized * runAwayDistance, walkPointRange, -1);
            agent.SetDestination(newPos);
        }

        agent.speed = runAwaySpeed;
        SmoothRotateTowards(agent.destination);
    }

    private void SwitchState(State newState)
    {
        currentState = newState;

        if (newState == State.Wander)
        {
            wanderTimer = wanderTime;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, runAwayRange);
    }
}
