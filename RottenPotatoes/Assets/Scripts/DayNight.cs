using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public float normalRotationDuration = 1200f; 
    public float fastRotationDuration = 60f;
    private float rotationDuration;
    private bool isFastRotation = false;

    void Start()
    {

        rotationDuration = normalRotationDuration;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            isFastRotation = !isFastRotation;
            rotationDuration = isFastRotation ? fastRotationDuration : normalRotationDuration;
        }

        float rotationSpeed = 360f / rotationDuration;

        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
