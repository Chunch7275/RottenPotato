using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlantHealthDisplay : MonoBehaviour
{
    public static Canvas mainCanvas;
    public static GameObject healthTextPrefab;

    public Vector3 offset = new Vector3(0, 2f, 0);

    private HealthSystem plantHealth;
    private Camera mainCamera;
    private TextMeshProUGUI healthTextInstance;

    void Awake()
    {
        plantHealth = GetComponent<HealthSystem>();
        mainCamera = Camera.main;

        if (mainCanvas == null)
        {
            Canvas canvasFound = FindObjectOfType<Canvas>();
            if (canvasFound == null)
            {
                GameObject canvasObj = new GameObject("Canvas", typeof(Canvas));
                canvasObj.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                canvasFound = canvasObj.GetComponent<Canvas>();
            }
            mainCanvas = canvasFound;
        }

        if (healthTextPrefab == null)
        {
            healthTextPrefab = Resources.Load<GameObject>("HealthText");
        }

        if (healthTextPrefab != null && mainCanvas != null)
        {
            GameObject textObj = Instantiate(healthTextPrefab, Vector3.zero, Quaternion.identity, mainCanvas.transform);
            healthTextInstance = textObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Missing HealthText prefab in Resources or Canvas in Scene.");
        }
    }

    void Update()
    {
        if (plantHealth != null && healthTextInstance != null)
        {
            healthTextInstance.text = plantHealth.CurrentHealth.ToString();

            Vector3 worldPosition = transform.position + offset;
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
            healthTextInstance.transform.position = screenPosition;
        }
    }

    void OnDestroy()
    {
        if (healthTextInstance != null)
        {
            Destroy(healthTextInstance.gameObject);
        }
    }
}
