using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAIController : MonoBehaviour
{
    private Bot bot;

    public float detectionRadius = 15f;
    public float attackRange = 2f;
    public int attackDamage = 1;
    public float attackCooldown = 1.5f;

    private float lastAttackTime = 0f;

    private GameObject currentTarget;

    void Start()
    {
        bot = GetComponent<Bot>();
    }

    void Update()
    {
        GameObject plant = FindClosestWithTag("Plant");
        GameObject player = FindClosestWithTag("Player");

        GameObject chosenTarget = null;

        if (plant != null)
        {
            chosenTarget = plant;
        }
        else if (player != null)
        {
            chosenTarget = player;
        }

        if (chosenTarget != currentTarget)
        {
            currentTarget = chosenTarget;
            bot.SetTarget(currentTarget);
        }

        if (currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (distanceToTarget <= attackRange)
            {
                Attack(currentTarget);
            }
            else
            {
                bot.MoveToTarget();
            }
        }
        else
        {
            bot.Wander();
        }
    }

    private void Attack(GameObject target)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            HealthSystem health = target.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }
            lastAttackTime = Time.time;
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
