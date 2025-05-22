using UnityEngine;
using UnityEngine.UI;

public class PlantUIManager : MonoBehaviour
{
    public Image[] plantImages = new Image[7];
    public Color normalColor = Color.white;
    public Color disabledColor = Color.gray;

    private GridBehavior grid;

    void Start()
    {
        GameObject gridGen = GameObject.Find("GridGen");
        if (gridGen != null)
        {
            grid = gridGen.GetComponent<GridBehavior>();
        }

        if (grid == null)
        {
            Debug.LogError("[PlantUIManager] GridBehavior not found on 'GridGen'");
        }
    }

    void Update()
    {
        if (grid == null) return;

        int[] costs = new int[]
        {
            grid.plant1Cost,
            grid.plant2Cost,
            grid.plant3Cost,
            grid.plant4Cost,
            grid.plant5Cost,
            grid.plant6Cost,
            grid.plant7Cost
        };

        for (int i = 0; i < 7; i++)
        {
            if (plantImages[i] == null) continue;

            bool affordable = grid.resourceAmount >= costs[i];
            plantImages[i].color = affordable ? normalColor : disabledColor;
        }
    }
}
