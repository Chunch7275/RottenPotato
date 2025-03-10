using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour


    //collisions destroy the grid that is made by the pathfinding. Set this on obstacles, make sure they are
   //rigidbodies and kenimatic.
{
    private void OnTriggerEnter(Collider other)

    {
        Debug.Log("Trigger with: " + other.name); 
        Destroy(other.gameObject);  
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name); 
        Destroy(collision.gameObject);  
    }
}

