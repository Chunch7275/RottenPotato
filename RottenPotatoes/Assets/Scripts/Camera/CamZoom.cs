using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoom : MonoBehaviour
{
    public float zoomSpeed = 2f; // Speed at which the camera zooms
    public float minZoom = 5f;   // Minimum zoom value (for perspective: FOV, for orthographic: size)
    public float maxZoom = 50f;  // Maximum zoom value (for perspective: FOV, for orthographic: size)

    private Camera cam; // Reference to the camera component

    // Start is called before the first frame update
    void Start()
    {
        // Cache the camera component
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleZooming();
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
