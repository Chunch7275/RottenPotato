using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedSelfDestruct : MonoBehaviour
{
    public float damageRange = 2f;
    public int damageAmount = 2;
    public float attackDelay = 0.5f; // Delay before the attack happens

    private bool hasAttacked = false;

    void Start()
    {
        Invoke(nameof(DoAttack), attackDelay);
    }

    void DoAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRange);
        foreach (Collider hit in hitColliders)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
        }

        hasAttacked = true;
        Destroy(gameObject); // Remove this object after the attack
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}
