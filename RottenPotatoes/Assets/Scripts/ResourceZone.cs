using UnityEngine;

public class ResourceZone : MonoBehaviour
{
    public GridBehavior gridBehavior;

    public int incrementAmount = 5;

    public float interval = 1f;

    private bool playerInside = false;
    private float timer = 0f;

    void Update()
    {
        if (playerInside && gridBehavior != null)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                gridBehavior.resourceAmount += incrementAmount;
                Debug.Log($"[ResourceZone] ResourceAmount increased to: {gridBehavior.resourceAmount}");
                timer = 0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // debug to confirm trigger working
        Debug.Log("[ResourceZone] Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            playerInside = true;
            timer = 0f;
            Debug.Log("[ResourceZone] Player entered resource zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            timer = 0f;
            Debug.Log("[ResourceZone] Player exited resource zone.");
        }
    }
}
