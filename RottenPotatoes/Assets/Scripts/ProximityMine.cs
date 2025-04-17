using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityMine : MonoBehaviour
{
    public float explosionDelay = 3f; // Time before explosion after first enemy contact
    public GameObject explosionEffectPrefab; // Assign a prefab for visual explosion

    private bool isTimerStarted = false;
    private List<GameObject> enemiesInContact = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!isTimerStarted)
            {
                StartCoroutine(ExplosionCountdown());
                isTimerStarted = true;
            }

            if (!enemiesInContact.Contains(other.gameObject))
            {
                enemiesInContact.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesInContact.Contains(other.gameObject))
            {
                enemiesInContact.Remove(other.gameObject);
            }
        }
    }

    IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(explosionDelay);

        Explode();
    }

    void Explode()
    {
        // Instantiate explosion visual effect if set
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Destroy all enemies still touching
        foreach (GameObject enemy in enemiesInContact)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        Debug.Log("BOOM!"); // You should see this in console when it explodes

        Destroy(gameObject); // Destroy the mine itself
    }
}
