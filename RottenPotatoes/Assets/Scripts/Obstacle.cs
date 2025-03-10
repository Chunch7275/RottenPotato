using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger with: " + other.name);  // Log the trigger event
        Destroy(other.gameObject);  // Destroy the prefab or other object
    }

    // For collision-based destruction
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name);  // Log the collision event
        Destroy(collision.gameObject);  // Destroy the object that collided with this
    }
}

