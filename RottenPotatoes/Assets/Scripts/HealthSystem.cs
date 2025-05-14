using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 5;
    public int CurrentHealth { get; private set; }

    public bool explodeOnDeath = false;
    public GameObject explosionPrefab;

    [Header("Regeneration Settings")]
    public bool regenerates = false;
    public int regenAmount = 1;
    public float regenInterval = 1f; // seconds between each regen tick

    private float regenTimer;

    private void Start()
    {
        CurrentHealth = maxHealth;
        regenTimer = regenInterval;
    }

    private void Update()
    {
        if (regenerates && CurrentHealth < maxHealth)
        {
            regenTimer -= Time.deltaTime;

            if (regenTimer <= 0f)
            {
                CurrentHealth = Mathf.Min(CurrentHealth + regenAmount, maxHealth);
                regenTimer = regenInterval;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (explodeOnDeath && explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
