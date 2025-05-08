using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAIController_Shooter : MonoBehaviour
{
    private Bot bot;

    public float detectionRadius = 15f;
    public float shootingDistance = 1f;
    public float attackCooldown = 2f;
    public int attackDamage = 1;

    public Transform shootPoint;

    private float lastAttackTime = 0f;
    private GameObject currentTarget;

    void Start()
    {
        bot = GetComponent<Bot>();
    }

    void Update()
    {
        GameObject plant = FindClosestWithTag("Plant");

        if (plant != null)
        {
            currentTarget = plant;
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (distanceToTarget > shootingDistance + 0.2f)
            {
                Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
                Vector3 stopPosition = currentTarget.transform.position - direction * shootingDistance;
                bot.MoveToPosition(stopPosition);
            }
            else
            {
                bot.SetTarget(null); // freeze movement
                FaceTarget(currentTarget);
                SimulateShoot(currentTarget);
            }
        }
        else
        {
            currentTarget = null;
            bot.Wander();
        }
    }

    private void SimulateShoot(GameObject target)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log($"[BotAIController_Shooter] Shooting at {target.name}");

            HealthSystem health = target.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }

            lastAttackTime = Time.time;
        }
    }

    private void FaceTarget(GameObject target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private GameObject FindClosestWithTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject obj in objs)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < detectionRadius && dist < minDist)
            {
                minDist = dist;
                closest = obj;
            }
        }
        return closest;
    }
}
