using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth { get; private set; }

    [Tooltip("Name of the scene to load when this enemy dies.")]
    public string sceneToLoadOnDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Optional: Destroy the enemy before loading scene, or comment this out if you don't want that
        Destroy(gameObject);

        // Load the specified scene
        if (!string.IsNullOrEmpty(sceneToLoadOnDeath))
        {
            SceneManager.LoadScene(sceneToLoadOnDeath);
        }
        else
        {
            Debug.LogWarning("[EnemyHealth] No scene name specified to load on death.");
        }
    }
}
