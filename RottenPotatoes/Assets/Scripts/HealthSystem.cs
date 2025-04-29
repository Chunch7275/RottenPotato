using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 5;
    public int CurrentHealth { get; private set; } // <- Public Getter

    public bool explodeOnDeath = false;
    public GameObject explosionPrefab; // Optional, set an explosion prefab in Inspector

    private void Start()
    {
        CurrentHealth = maxHealth;
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
