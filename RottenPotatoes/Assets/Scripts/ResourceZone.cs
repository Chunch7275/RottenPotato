using UnityEngine;
using System.Collections.Generic;

public class ResourceZone : MonoBehaviour
{
    public GridBehavior gridBehavior;
    public int incrementAmount = 5;
    public float interval = 1f;

    private float timer = 0f;
    private bool isIncrementing = false;

    // Track all valid objects in the zone
    private HashSet<Collider> validObjectsInZone = new HashSet<Collider>();

    void Update()
    {
        if (isIncrementing && gridBehavior != null)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                gridBehavior.resourceAmount += incrementAmount;
                Debug.Log($"[ResourceZone] ResourceAmount increased to: {gridBehavior.resourceAmount}");
                timer = 0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Plant"))
        {
            if (validObjectsInZone.Add(other)) // Add returns true if the item was not already in the set
            {
                if (!isIncrementing)
                {
                    isIncrementing = true;
                    timer = 0f;
                    Debug.Log("[ResourceZone] Incrementing started.");
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (validObjectsInZone.Remove(other)) // Remove and check if set is now empty
        {
            if (validObjectsInZone.Count == 0)
            {
                isIncrementing = false;
                Debug.Log("[ResourceZone] Incrementing stopped.");
            }
        }
    }
}
