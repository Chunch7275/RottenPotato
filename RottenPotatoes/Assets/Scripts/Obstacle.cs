using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Collisions destroy the grid that is made by the pathfinding. 
    // Set this on obstacles, make sure they are rigidbodies and kinematic.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grid"))
        {
            Debug.Log("Trigger with: " + other.name);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grid"))
        {
            Debug.Log("Collision with: " + collision.gameObject.name);
            Destroy(collision.gameObject);
        }
    }
}

