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
    private Transform currentTarget;
    private NavMeshAgent agent;
    private float lastAttackTime = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check for nearby plant objects
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
        Transform closestPlant = null;
        float closestPlantDistance = Mathf.Infinity;

        foreach (GameObject plant in plants)
        {
            float distance = Vector3.Distance(transform.position, plant.transform.position);
            if (distance < detectionRadius && distance < closestPlantDistance)
            {
                closestPlant = plant.transform;
                closestPlantDistance = distance;
            }
        }

        // Choose target: plant if one is nearby, otherwise player
        currentTarget = (closestPlant != null) ? closestPlant : player;

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        if (distanceToTarget <= detectionRadius)
        {
            // Chase current target
            agent.SetDestination(currentTarget.position);

            // Face target
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Attack if in range and in front
            if (distanceToTarget <= attackRange && IsTargetInFront())
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackTarget();
                    lastAttackTime = Time.time;
                }
            }
        }
        else
        {
            agent.ResetPath();
        }
    }

    bool IsTargetInFront()
    {
        Vector3 toTarget = (currentTarget.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, toTarget);
        return angle < fieldOfViewAngle / 2f;
    }

    void AttackTarget()
    {
        if (currentTarget.CompareTag("Player"))
        {
            Debug.Log("player attacked");
        }
        else if (currentTarget.CompareTag("Plant"))
        {
            Debug.Log("plant attacked");
        }
    }
}
