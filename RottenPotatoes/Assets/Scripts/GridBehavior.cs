using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    public GameObject followerSpherePrefab; // Reference to the sphere prefab
    private GameObject followerSphere;      // The sphere instance
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
    public List<GameObject> path = new List<GameObject>();


    // Start is called before the first frame update
    void Awake()
    {
        gridArray = new GameObject[columns, rows];

        if (gridPrefab)
            GenerateGrid();
        else print("missing grid");


        //move

        if(followerSpherePrefab)
        {
            followerSphere = Instantiate(followerSpherePrefab, gridArray[startX, startY].transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Missing Root Prefab");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(FindDistance)
        {
            SetDistance();
            SetPath();
            FindDistance = false;

        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
        {
            DetectGridClick();
        }
    }
    void GenerateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                // Check if the gridSpherePrefab is assigned
                if (gridPrefab)
                {
                    // Instantiate the prefab for each grid cell
                    GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);

                    // Set the name and parent
                    obj.name = $"Grid_{i}_{j}";
                    obj.transform.SetParent(gameObject.transform);

                    // Add the GridStat component (or any other relevant components)
                    obj.AddComponent<GridStat>();
                    obj.GetComponent<GridStat>().x = i;
                    obj.GetComponent<GridStat>().y = j;

                    // Add the object to the grid array
                    gridArray[i, j] = obj;
                }
                else
                {
                    Debug.Log("Grid Sphere Prefab is missing!");
                }
            }
        }
    }
    void SetDistance()
    {
        InitialSetup();
        int x = startX;
        int y = startY;
        int[] testArray = new int[rows * columns];
        for (int step = 1; step < rows * columns; step++)
        {
            foreach (GameObject obj in gridArray)
            {
                if (obj && obj.GetComponent<GridStat>().visited == step - 1)
                    TestFourDirections(obj.GetComponent<GridStat>().x, obj.GetComponent<GridStat>().y, step);

            }
        }


    }
    void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();  // Ensure the path is cleared at the start of this function

        if (gridArray[endX, endY] != null && gridArray[endX, endY].GetComponent<GridStat>().visited > 0)
        {
            path.Add(gridArray[x, y]);  // Add the endpoint to the path
            step = gridArray[x, y].GetComponent<GridStat>().visited - 1;
        }
        else
        {
            Debug.Log("Move not available");
            return;
        }

        // Calculate the path by working backward from the endpoint to the start
        for (int i = step; step > -1; step--)
        {
            tempList.Clear();  // Clear the temp list before adding new options

            if (TestDirection(x, y, step, 1)) tempList.Add(gridArray[x, y + 1]);
            if (TestDirection(x, y, step, 2)) tempList.Add(gridArray[x + 1, y]);
            if (TestDirection(x, y, step, 3)) tempList.Add(gridArray[x, y - 1]);
            if (TestDirection(x, y, step, 4)) tempList.Add(gridArray[x - 1, y]);

            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            if (tempObj != null)
            {
                path.Add(tempObj);  // Add the closest valid point to the path
                x = tempObj.GetComponent<GridStat>().x;
                y = tempObj.GetComponent<GridStat>().y;
            }
        }

        // Reverse the path so it moves from start to end
        path.Reverse();

        StartCoroutine(MoveAlongPath());
    }




    bool TestDirection(int x, int y, int step, int direction)
    {
        switch (direction)
        {
            case 4: // Left
                if (x - 1 > -1 && gridArray[x - 1, y] != null && gridArray[x - 1, y].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;

            case 3: // Down
                if (y - 1 > -1 && gridArray[x, y - 1] != null && gridArray[x, y - 1].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;

            case 2: // Right
                if (x + 1 < columns && gridArray[x + 1, y] != null && gridArray[x + 1, y].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;

            case 1: // Up
                if (y + 1 < rows && gridArray[x, y + 1] != null && gridArray[x, y + 1].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;
        }
        return false;
    }

    void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1))
            SetVisited(x, y + 1, step);

        if (TestDirection(x, y, -1, 2))
            SetVisited(x + 1, y, step);

        if (TestDirection(x, y, -1, 3))
            SetVisited(x, y - 1, step);

        if (TestDirection(x, y, -1, 4))
            SetVisited(x - 1, y, step);
    }


    void SetVisited(int x, int y, int step)
    {
        if (gridArray[x, y] != null)
        {
            gridArray[x, y].GetComponent<GridStat>().visited = step;
        }
    }
    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = scale * rows * columns;
        int indexnumber = -1;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null) // Check for null entries
            {
                float distance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    indexnumber = i;
                }
            }
        }

        return indexnumber != -1 ? list[indexnumber] : null; // Return the closest, or null if nothing found
    }

    IEnumerator MoveAlongPath()
    {
        foreach (GameObject waypoint in path)
        {
            if (waypoint == null) continue; // Skip null waypoints

            // Move towards the current waypoint
            while (Vector3.Distance(followerSphere.transform.position, waypoint.transform.position) > 0.1f)
            {
                followerSphere.transform.position = Vector3.MoveTowards(followerSphere.transform.position, waypoint.transform.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Small pause at each waypoint
            yield return new WaitForSeconds(0.1f);
        }

        // After reaching the final waypoint, update startX and startY to match the endX and endY
        startX = endX;
        startY = endY;

        Debug.Log($"New Start Position: ({startX}, {startY})");
    }


    void InitialSetup()
    {
        // Reset the visited state of all grid cells
        foreach (GameObject obj in gridArray)
        {
            if (obj != null) // Ensure the cell is not null (in case it has been deleted)
            {
                obj.GetComponent<GridStat>().visited = -1;
            }
        }

        // Mark the starting cell as visited (step 0)
        if (gridArray[startX, startY] != null)
        {
            gridArray[startX, startY].GetComponent<GridStat>().visited = 0;
        }
    }
    void DetectGridClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;
            if (clickedObject.GetComponent<GridStat>() != null)
            {
                GridStat gridStat = clickedObject.GetComponent<GridStat>();
                endX = gridStat.x;
                endY = gridStat.y;
                FindDistance = true;
            }
            else
            {
                Debug.Log("Clicked object is not part of the grid.");
            }
        }
    }

    }
