using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceDisplay : MonoBehaviour
{
    private GridBehavior gridBehavior;
    private TextMeshProUGUI resourceText;

    void Awake()
    {
        resourceText = GetComponent<TextMeshProUGUI>();

        if (gridBehavior == null)
        {
            gridBehavior = FindObjectOfType<GridBehavior>();
        }

        if (gridBehavior == null)
        {
            Debug.LogError("No GridBehavior found in the scene!");
        }
    }

    void Update()
    {
        if (gridBehavior != null && resourceText != null)
        {
            resourceText.text = "Resources: " + gridBehavior.resourceAmount.ToString();
        }
    }
}
