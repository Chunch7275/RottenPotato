using UnityEngine;

public class GridStat : MonoBehaviour
{
    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public GameObject parent;
    public bool isWalkable = true; // This flag determines if a grid cell can be walked on
    public int visited = -1; // For marking in pathfinding

    public int fCost
    {
        get { return gCost + hCost; }
    }
}