using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour

{
    [SerializeField] private Material hoverMaterial;    
    private Material originalMaterial;                   
    private Renderer objRenderer;

    // Collisions destroy the grid that is made by the pathfinding. 
    // Set this on obstacles, make sure they are rigidbodies and kinematic.

    private void Awake()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer != null)
        {
            originalMaterial = objRenderer.material;
        }
    }

    private void OnMouseEnter()
    {
        if (objRenderer != null && hoverMaterial != null)
        {
            //We'll need to set the Tree Prefab's material's Alphas to a lower setting when this fires.  That should allow the trees to become translucent when hovering over them.  -5/1/25 Ben Heifetz
            objRenderer.material = hoverMaterial;
        }
    }

    private void OnMouseExit()
    {
        if (objRenderer != null && originalMaterial != null)
        {
            objRenderer.material = originalMaterial;
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
