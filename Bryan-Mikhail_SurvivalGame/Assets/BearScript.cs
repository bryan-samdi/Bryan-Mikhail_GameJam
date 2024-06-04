using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearScript : MonoBehaviour
{
    public Transform player;
    public float wanderSpeed = 1.5f;
    public float chaseSpeed = 3.5f;
    public float attackRange = 2.0f;
    public float chaseRange = 10.0f;
    public float wanderTime = 5.0f;
    public int attackDamage = 10;
    public float attackCooldown = 1.0f;
    Animator bearAnim;

    private NavMeshAgent agent;
    private float wanderTimer;
    private float attackTimer;
    private enum State { Wander, Chase, Attack }
    private State currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wanderTimer = wanderTime;
        attackTimer = 0f;
        SwitchState(State.Wander);
        bearAnim = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        attackTimer -= Time.deltaTime;

        switch (currentState)
        {
            case State.Wander:
                Wander();
                if (distanceToPlayer <= chaseRange)
                {
                    SwitchState(State.Chase);
                }
                break;

            case State.Chase:
                Chase();
                if (distanceToPlayer <= attackRange)
                {
                    SwitchState(State.Attack);
                }
                else if (distanceToPlayer > chaseRange)
                {
                    SwitchState(State.Wander);
                }
                break;

            case State.Attack:
                Attack();
                if (distanceToPlayer > attackRange)
                {
                    SwitchState(State.Chase);
                }
                break;
        }
    }

    void Wander()
    {
        bearAnim.SetTrigger("BearWalk");
        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderTime)
        {
            Vector3 newPos = RandomNavSphere(transform.position, chaseRange, -1);
            agent.SetDestination(newPos);
            wanderTimer = 0;
        }
        agent.speed = wanderSpeed;
    }

    void Chase()
    {
        bearAnim.SetTrigger("BearChase");
        agent.SetDestination(player.position);
        agent.speed = chaseSpeed;
    }

    void Attack()
    {
        bearAnim.SetTrigger("BearAttack");

        agent.SetDestination(transform.position); 
        if (attackTimer <= 0f)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            attackTimer = attackCooldown;

        }
    }

    void SwitchState(State newState)
    {
        currentState = newState;
        if (newState == State.Wander)
        {
            wanderTimer = wanderTime;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
