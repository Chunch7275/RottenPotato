using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Obstacle : MonoBehaviour

{
    private Material[] materials;
    private Renderer objRenderer;

    // Collisions destroy the grid that is made by the pathfinding. 
    // Set this on obstacles, make sure they are rigidbodies and kinematic.

    private void Awake()
    {
        objRenderer = GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        if (objRenderer != null)
        {
            materials = objRenderer.materials;
            for(int i = 0; i < materials.Length; i++)
            {
                materials[i].color = new Color(materials[i].color.r, materials[i].color.b, materials[i].color.g, 0.25f);
            }
        }
    }

    private void OnMouseExit()
    {
            materials = objRenderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = new Color(materials[i].color.r, materials[i].color.b, materials[i].color.g, 1);
            }
    }

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
