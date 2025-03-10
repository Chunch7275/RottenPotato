using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAM : MonoBehaviour
{
    public float panSpeed = 20f; // Speed at which the camera pans
    public float zoomSpeed = 2f; // Speed at which the camera zooms
    public float minZoom = 5f;   // Minimum zoom value (for perspective: FOV, for orthographic: size)
    public float maxZoom = 50f;  // Maximum zoom value (for perspective: FOV, for orthographic: size)

    private Vector3 dragOrigin;  // The point where the drag started
    private bool isPanning = false;

    private Camera cam; // Reference to the camera component

    void Start()
    {
        // Cache the camera component
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePanning();
        HandleZooming();
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

        // Move the camera based on the difference, scaled by panSpeed and deltaTime
        Vector3 move = new Vector3(difference.x * panSpeed * Time.deltaTime, 0, difference.y * panSpeed * Time.deltaTime);
        transform.Translate(move, Space.World);

        // Update dragOrigin to the current mouse position for the next frame
        dragOrigin = currentMousePosition;
    }

    void HandleZooming()
    {
        // Get scroll input (positive for zooming in, negative for zooming out)
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (cam.orthographic)
        {
            // For orthographic cameras, adjust the orthographic size
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
        else
        {
            // For perspective cameras, adjust the field of view (FOV)
            cam.fieldOfView -= scroll * zoomSpeed * 10f; // Multiply to make FOV adjustments more sensitive
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
        }
    }
}
