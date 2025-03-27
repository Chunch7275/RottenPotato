using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasingAI : MonoBehaviour
{
    public float detectionRadius = 10f;
    public float attackRange = 2f;
    public float fieldOfViewAngle = 60f;
    public float attackCooldown = 1.5f;

    private Transform player;
    private NavMeshAgent agent;
    private float lastAttackTime = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Chase player
            agent.SetDestination(player.position);

            // Face player
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Check if player is in front and in attack range
            if (distanceToPlayer <= attackRange && IsPlayerInFront())
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }
        }
        else
        {
            // Stop moving when player is out of detection
            agent.ResetPath();
        }
    }

    bool IsPlayerInFront()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, toPlayer);
        return angle < fieldOfViewAngle / 2f;
    }

    void AttackPlayer()
    {
        Debug.Log("player attacked");
    }
}