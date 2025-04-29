using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damageRange = 2f;
    public int damageAmount = 2;
    public float damageCooldown = 1f;

    private float lastDamageTime;

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRange);
        foreach (Collider hit in hitColliders)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null && Time.time >= lastDamageTime + damageCooldown)
            {
                enemy.TakeDamage(damageAmount);
                lastDamageTime = Time.time;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}
