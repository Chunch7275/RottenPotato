using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    public GameObject followerSpherePrefab;
    private GameObject followerSphere;
    public float moveSpeed = 2.0f;

    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    public GameObject[,] gridArray;
    public int startX = 0;
    public int startY = 0;
    public int endX = 2;
    public int endY = 2;
    public bool FindDistance = false;
    private bool pathChanged = true;
  private bool isMoving = false; // New flag to check if sphere is moving
    public List<GameObject> path = new List<GameObject>();

    void Awake()
    {
        gridArray = new GameObject[columns, rows];

        if (gridPrefab)
            GenerateGrid();
        else
            print("missing grid");

        if (followerSpherePrefab)
        {
            followerSphere = Instantiate(followerSpherePrefab, gridArray[startX, startY].transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Missing Root Prefab");
        }
    }

    void Update()
    {
        if (FindDistance && pathChanged && !isMoving) // Only find path if not moving
        {
            AStarPathfinding();
            pathChanged = false;
            FindDistance = false;
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0) && !isMoving) // Only detect click if not moving
        {
            DetectGridClick();
            pathChanged = true;
        }
    }

    void GenerateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (gridPrefab)
                {
                    GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);
                    obj.name = $"Grid_{i}_{j}";
                    obj.transform.SetParent(gameObject.transform);
                    obj.AddComponent<GridStat>();
                    obj.GetComponent<GridStat>().x = i;
                    obj.GetComponent<GridStat>().y = j;
                    gridArray[i, j] = obj;
                }
                else
                {
                    Debug.Log("Grid Prefab is missing!");
                }
            }
        }
    }

    void AStarPathfinding()
    {
        InitialSetup();
        List<GameObject> openSet = new List<GameObject>();
        HashSet<GameObject> closedSet = new HashSet<GameObject>();

        GameObject startNode = gridArray[startX, startY];
        GameObject endNode = gridArray[endX, endY];

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            GameObject current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                GridStat currentStat = current.GetComponent<GridStat>();
                GridStat nextStat = openSet[i].GetComponent<GridStat>();
                if (nextStat.fCost < currentStat.fCost || (nextStat.fCost == currentStat.fCost && nextStat.hCost < currentStat.hCost))
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            foreach (GameObject neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;

                GridStat neighborStat = neighbor.GetComponent<GridStat>();
                GridStat currentStat = current.GetComponent<GridStat>();

                if (!neighborStat.isWalkable || closedSet.Contains(neighbor)) continue; // Skip unwalkable or already closed neighbors

                int newCostToNeighbor = currentStat.gCost + GetDistance(current, neighbor);
                if (newCostToNeighbor < neighborStat.gCost || !openSet.Contains(neighbor))
                {
                    neighborStat.gCost = newCostToNeighbor;
                    neighborStat.hCost = GetDistance(neighbor, endNode);
                    neighborStat.parent = current;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
    }

    void RetracePath(GameObject startNode, GameObject endNode)
    {
        path.Clear();
        GameObject currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.GetComponent<GridStat>().parent;
        }
        path.Add(startNode);
        path.Reverse();

        StartCoroutine(MoveAlongPath());
    }

    IEnumerator MoveAlongPath()
    {
        isMoving = true; // Mark the sphere as moving

        foreach (GameObject waypoint in path)
        {
            if (waypoint == null) continue;

            while (Vector3.Distance(followerSphere.transform.position, waypoint.transform.position) > 0.1f)
            {
                followerSphere.transform.position = Vector3.MoveTowards(followerSphere.transform.position, waypoint.transform.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }

        startX = endX;
        startY = endY;

        isMoving = false; // Mark the sphere as not moving anymore

        // Reset pathfinding trigger
        pathChanged = true;
        FindDistance = false;
        Debug.Log($"New Start Position: ({startX}, {startY})");
    }

    void InitialSetup()
    {
        foreach (GameObject obj in gridArray)
        {
            if (obj != null)
            {
                GridStat stat = obj.GetComponent<GridStat>();
                stat.visited = -1;
                stat.gCost = stat.hCost = 0;
                stat.parent = null;
            }
        }

        gridArray[startX, startY].GetComponent<GridStat>().visited = 0;
    }

    void DetectGridClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Ray from the camera to mouse position
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) // If the ray hits something
        {
            // Calculate grid coordinates based on the hit position
            Vector3 hitPos = hit.point;
            int x = Mathf.RoundToInt((hitPos.x - leftBottomLocation.x) / scale);
            int y = Mathf.RoundToInt((hitPos.z - leftBottomLocation.z) / scale);

            // Check if the click is within the grid's bounds
            if (x >= 0 && x < columns && y >= 0 && y < rows)
            {
                // Set the end coordinates and allow movement
                endX = x;
                endY = y;

                // Allow A* to run again
                FindDistance = true;
                pathChanged = true;
                Debug.Log($"Clicked grid at: ({endX}, {endY})");
            }
        }
    }


    List<GameObject> GetNeighbors(GameObject node)
    {
        List<GameObject> neighbors = new List<GameObject>();
        GridStat stat = node.GetComponent<GridStat>();
        int x = stat.x;
        int y = stat.y;

        // Add horizontal and vertical neighbors
        AddNeighborIfValid(neighbors, x + 1, y);  // Right
        AddNeighborIfValid(neighbors, x - 1, y);  // Left
        AddNeighborIfValid(neighbors, x, y + 1);  // Up
        AddNeighborIfValid(neighbors, x, y - 1);  // Down

        // Add diagonal neighbors
        AddNeighborIfValid(neighbors, x + 1, y + 1);  // Top-right
        AddNeighborIfValid(neighbors, x - 1, y + 1);  // Top-left
        AddNeighborIfValid(neighbors, x + 1, y - 1);  // Bottom-right
        AddNeighborIfValid(neighbors, x - 1, y - 1);  // Bottom-left

        return neighbors;
    }

    void AddNeighborIfValid(List<GameObject> neighbors, int x, int y)
    {
        // Check if within grid bounds
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            GameObject neighbor = gridArray[x, y];

            if (neighbor != null && neighbor.GetComponent<GridStat>().isWalkable) // Check if not destroyed and walkable
            {
                neighbors.Add(neighbor);
            }
        }
    }


    int GetDistance(GameObject a, GameObject b)
    {
        GridStat aStat = a.GetComponent<GridStat>();
        GridStat bStat = b.GetComponent<GridStat>();
        int dstX = Mathf.Abs(aStat.x - bStat.x);
        int dstY = Mathf.Abs(aStat.y - bStat.y);
        return dstX + dstY;
    }
}
