using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAIController_Shooter : MonoBehaviour
{
    private Bot bot;

    public float detectionRadius = 15f;
    public float shootingRange = 10f;
    public int attackDamage = 1;
    public float attackCooldown = 2f;

    public Transform shootPoint; // Optional: for effects like muzzle flash

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

            if (distanceToTarget <= shootingRange)
            {
                bot.SetTarget(null); // stop pathing
                FaceTarget(currentTarget);
                SimulateShoot(currentTarget);
            }
            else
            {
                bot.SetTarget(currentTarget);
                bot.MoveToTarget();
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
            // Optional: trigger visual FX at shootPoint
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
        direction.y = 0f; // prevent vertical tilting
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
