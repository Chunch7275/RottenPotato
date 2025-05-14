using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HealthSystem trackedObject; // Assign this in the Inspector
    private bool gameIsOver = false;

    void Update()
    {
        // Check if the tracked object has been destroyed
        if (!gameIsOver && trackedObject == null)
        {
            Debug.Log("Game Over");
            gameIsOver = true;
        }
    }
}
