using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    public int resourceAmount = 100;  // Starting resources
    public int plant1Cost = 10;
    public int plant2Cost = 10;
    public int plant3Cost = 10;
    public int plant4Cost = 10;
    public int plant5Cost = 10;
    public int plant6Cost = 10;
    public int plant7Cost = 10;
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
    public GameObject trailPrefab;
    private bool isMoving = false;
    public List<GameObject> path = new List<GameObject>();

    public GameObject highlightIndicatorPrefab;
    private GameObject currentHighlight;
    public Material onGridMaterial;
    public Material offGridMaterial;
    public Material key1Material;
    public Material key2Material;
    public Material key3Material;
    public Material key4Material;
    public Material key5Material;
    public Material key6Material;
    public Material key7Material;
    public Material key8Material;
    public Material key9Material;
    public GameObject key1Prefab;
    public GameObject key2Prefab;
    public GameObject key3Prefab;
    public GameObject key4Prefab;
    public GameObject key5Prefab;
    public GameObject key6Prefab;
    public GameObject key7Prefab;
    private bool isKey1Toggled = false;
    private bool isKey2Toggled = false;
    private bool isKey3Toggled = false;
    private bool isKey4Toggled = false;
    private bool isKey5Toggled = false;
    private bool isKey6Toggled = false;
    private bool isKey7Toggled = false;

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
        if (FindDistance && pathChanged && !isMoving)
        {
            AStarPathfinding();
            pathChanged = false;
            FindDistance = false;
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0) && !isMoving)
        {
            DetectGridClick();
            pathChanged = true;
        }
        DetectGridHover();
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

    void DetectGridHover()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isKey1Toggled = !isKey1Toggled;
            isKey2Toggled = false;
            isKey3Toggled = false;
            isKey4Toggled = false;
            isKey5Toggled = false;
            isKey6Toggled = false;
            isKey7Toggled = false;

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isKey2Toggled = !isKey2Toggled;
            isKey1Toggled = false;
            isKey3Toggled = false;
            isKey4Toggled = false;
            isKey5Toggled = false;
            isKey6Toggled = false;
            isKey7Toggled = false;

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isKey3Toggled = !isKey3Toggled;
            isKey1Toggled = false;
            isKey2Toggled = false;
            isKey4Toggled = false;
            isKey5Toggled = false;
            isKey6Toggled = false;
            isKey7Toggled = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            isKey4Toggled = !isKey4Toggled;
            isKey1Toggled = false;
            isKey2Toggled = false;
            isKey3Toggled = false;
            isKey5Toggled = false;
            isKey6Toggled = false;
            isKey7Toggled = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            isKey5Toggled = !isKey5Toggled;
            isKey1Toggled = false;
            isKey2Toggled = false;
            isKey3Toggled = false;
            isKey4Toggled = false;
            isKey6Toggled = false;
            isKey7Toggled = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            isKey6Toggled = !isKey6Toggled;
            isKey1Toggled = false;
            isKey2Toggled = false;
            isKey3Toggled = false;
            isKey4Toggled = false;
            isKey5Toggled = false;
            isKey7Toggled = false;

        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            isKey7Toggled = !isKey7Toggled;
            isKey1Toggled = false;
            isKey2Toggled = false;
            isKey3Toggled = false;
            isKey4Toggled = false;
            isKey5Toggled = false;
            isKey6Toggled = false;

        }


        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPos = hit.point;
            int x = Mathf.RoundToInt((hitPos.x - leftBottomLocation.x) / scale);
            int y = Mathf.RoundToInt((hitPos.z - leftBottomLocation.z) / scale);

            if (x >= 0 && x < columns && y >= 0 && y < rows)
            {
                Vector3 hoverPosition = gridArray[x, y].transform.position;
                hoverPosition.y += 0.01f;

                if (currentHighlight == null)
                {
                    currentHighlight = Instantiate(highlightIndicatorPrefab, hoverPosition, Quaternion.identity);
                }
                else
                {
                    currentHighlight.transform.position = hoverPosition;
                }


                if (isKey1Toggled)
                {
                    currentHighlight.GetComponent<Renderer>().material = key1Material;
                }
                else if (isKey2Toggled)
                {
                    currentHighlight.GetComponent<Renderer>().material = key2Material;
                }
                else if (isKey3Toggled)
                {
                    currentHighlight.GetComponent<Renderer>().material = key3Material;
                }
                else if (isKey4Toggled)
                {
                    currentHighlight.GetComponent<Renderer>().material = key4Material;
                }
                else if (isKey5Toggled)
                {
                    currentHighlight.GetComponent<Renderer>().material = key5Material;
                }
                else if (isKey6Toggled)
                {
                    currentHighlight.GetComponent<Renderer>().material = key6Material;
                }
                else if (isKey7Toggled)
                {
                    currentHighlight.GetComponent<Renderer>().material = key7Material;
                }

                else
                {
                    currentHighlight.GetComponent<Renderer>().material = onGridMaterial;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (isKey1Toggled)
                    {
                        if (resourceAmount >= plant1Cost)
                        {
                            Instantiate(key1Prefab, hoverPosition, Quaternion.identity);
                            resourceAmount -= plant1Cost;
                            Debug.Log("Placed plant. Resources left: " + resourceAmount);
                        }
                        else
                        {
                            Debug.Log("Not enough resources to place a plant!");
                        }
                    }
                    else if (isKey2Toggled)
                    {
                        if (resourceAmount >= plant2Cost)
                        {
                            Instantiate(key2Prefab, hoverPosition, Quaternion.identity);
                            resourceAmount -= plant2Cost;
                            Debug.Log("Placed plant. Resources left: " + resourceAmount);
                        }
                        else
                        {
                            Debug.Log("Not enough resources to place a plant!");
                        }
                    }
                    else if (isKey3Toggled)
                    {
                        if (resourceAmount >= plant3Cost)
                        {
                            Instantiate(key3Prefab, hoverPosition, Quaternion.identity);
                            resourceAmount -= plant3Cost;
                            Debug.Log("Placed plant. Resources left: " + resourceAmount);
                        }
                        else
                        {
                            Debug.Log("Not enough resources to place a plant!");
                        }
                    }
                    else if (isKey4Toggled)
                    {
                        if (resourceAmount >= plant4Cost)
                        {
                            Instantiate(key4Prefab, hoverPosition, Quaternion.identity);
                            resourceAmount -= plant4Cost;
                            Debug.Log("Placed plant. Resources left: " + resourceAmount);
                        }
                        else
                        {
                            Debug.Log("Not enough resources to place a plant!");
                        }
                    }
                    else if (isKey5Toggled)
                    {
                        if (resourceAmount >= plant5Cost)
                        {
                            Instantiate(key5Prefab, hoverPosition, Quaternion.identity);
                            resourceAmount -= plant5Cost;
                            Debug.Log("Placed plant. Resources left: " + resourceAmount);
                        }
                        else
                        {
                            Debug.Log("Not enough resources to place a plant!");
                        }
                    }
                    else if (isKey6Toggled)
                    {
                        if (resourceAmount >= plant6Cost)
                        {
                            Instantiate(key6Prefab, hoverPosition, Quaternion.identity);
                            resourceAmount -= plant6Cost;
                            Debug.Log("Placed plant. Resources left: " + resourceAmount);
                        }
                        else
                        {
                            Debug.Log("Not enough resources to place a plant!");
                        }
                    }
                    else if (isKey7Toggled)
                    {
                        if (resourceAmount >= plant7Cost)
                        {
                            Instantiate(key7Prefab, hoverPosition, Quaternion.identity);
                            resourceAmount -= plant7Cost;
                            Debug.Log("Placed plant. Resources left: " + resourceAmount);
                        }
                        else
                        {
                            Debug.Log("Not enough resources to place a plant!");
                        }
                    }


                    isKey1Toggled = false;
                    isKey2Toggled = false;
                    isKey3Toggled = false;
                    isKey4Toggled = false;
                    isKey5Toggled = false;
                    isKey6Toggled = false;
                    isKey6Toggled = false;
                    isKey7Toggled = false;

                }
            }
            else
            {
                if (currentHighlight != null)
                {
                    currentHighlight.GetComponent<Renderer>().material = offGridMaterial;
                }
            }
        }
        else
        {
            if (currentHighlight != null)
            {
                currentHighlight.GetComponent<Renderer>().material = offGridMaterial;
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

                if (!neighborStat.isWalkable || closedSet.Contains(neighbor)) continue;

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
        if (resourceAmount <= 0)
        {
            Debug.Log("Not enough resources to start movement.");
            yield break;
        }

        isMoving = true;

        GameObject previousNode = null;

        foreach (GameObject waypoint in path)
        {
            if (waypoint == null) continue;

            // Stop BEFORE moving if out of resources
            if (previousNode != null && resourceAmount <= 0)
            {
                Debug.Log("Movement stopped due to insufficient resources.");
                break;
            }

            Vector3 targetPosition = waypoint.transform.position;

            while (Vector3.Distance(followerSphere.transform.position, targetPosition) > 0.1f)
            {
                followerSphere.transform.position = Vector3.MoveTowards(followerSphere.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            if (previousNode != null && trailPrefab != null)
            {
                Instantiate(trailPrefab, previousNode.transform.position, Quaternion.identity);
                resourceAmount -= 1;
            }

            previousNode = waypoint;
            yield return new WaitForSeconds(0.1f);
        }

        if (previousNode != null)
        {
            GridStat stoppedStat = previousNode.GetComponent<GridStat>();
            startX = stoppedStat.x;
            startY = stoppedStat.y;
        }

        isMoving = false;
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPos = hit.point;
            int x = Mathf.RoundToInt((hitPos.x - leftBottomLocation.x) / scale);
            int y = Mathf.RoundToInt((hitPos.z - leftBottomLocation.z) / scale);

            if (x >= 0 && x < columns && y >= 0 && y < rows)
            {
                endX = x;
                endY = y;

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

        AddNeighborIfValid(neighbors, x + 1, y);
        AddNeighborIfValid(neighbors, x - 1, y);
        AddNeighborIfValid(neighbors, x, y + 1);
        AddNeighborIfValid(neighbors, x, y - 1);

        AddNeighborIfValid(neighbors, x + 1, y + 1);
        AddNeighborIfValid(neighbors, x - 1, y + 1);
        AddNeighborIfValid(neighbors, x + 1, y - 1);
        AddNeighborIfValid(neighbors, x - 1, y - 1);

        return neighbors;
    }

    void AddNeighborIfValid(List<GameObject> neighbors, int x, int y)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            GameObject neighbor = gridArray[x, y];

            if (neighbor != null && neighbor.GetComponent<GridStat>().isWalkable)
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
