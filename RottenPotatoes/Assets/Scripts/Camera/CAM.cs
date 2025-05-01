using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAM : MonoBehaviour
{
    public float panSpeed = 20f; // Speed at which the camera pans

    private Vector3 dragOrigin;  // The point where the drag started
    private bool isPanning = false;

    // Update is called once per frame
    void Update()
    {
        HandlePanning();
    }

    void HandlePanning()
    {
        // Check if Shift is held down and left mouse button is pressed
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            // Capture the point where the mouse was clicked
            dragOrigin = Input.mousePosition;
            isPanning = true;
        }

        // If the mouse button is released, stop panning
        if (Input.GetMouseButtonUp(0))
        {
            isPanning = false;
        }

        // If we are panning the camera
        if (isPanning)
        {
            PanCamera();
        }
    }

    void PanCamera()
    {
        // Calculate the difference in mouse position since the drag started
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 difference = dragOrigin - currentMousePosition;

        //Rotating the input to fix diagonal panning --- 3/23/25 Ben Heifetz
        Vector3 rotationFixVector = new Vector3(0, 45.0f, 0);
        Quaternion rotationFix = Quaternion.Euler(rotationFixVector);


        // Move the camera based on the difference, scaled by panSpeed and deltaTime
        Vector3 move = new Vector3(difference.x * panSpeed * Time.deltaTime, 0, difference.y * panSpeed * Time.deltaTime);
        transform.Translate(rotationFix * move, Space.World);

        // Update dragOrigin to the current mouse position for the next frame
        dragOrigin = currentMousePosition;
    }
}
