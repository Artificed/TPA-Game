using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [SerializeField] Vector2Int startCords;
    public Vector2Int StartCords { get { return startCords; } }

    [SerializeField] Vector2Int targetCords;
    public Vector2Int TargetCords { get { return targetCords; } }
    
    Tile startNode;
    Tile targetNode;
    Tile currentNode;

    List<Tile> openSet = new List<Tile>();
    HashSet<Tile> closedSet = new HashSet<Tile>();

    GridManager gridManager;
    Dictionary<Vector2Int, Tile> grid = new Dictionary<Vector2Int, Tile>();

    Vector2Int[] searchOrder = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            grid = gridManager.Grid;
        }
    }

    public List<Tile> GetNewPath()
    {
        return GetNewPath(startCords);
    }

    public List<Tile> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();

        AStarSearch(coordinates);
        return BuildPath();
    }

    void AStarSearch(Vector2Int coordinates)
    {
        if (startNode.Blocked || targetNode.Blocked)
        {
            return;
        }

        openSet.Clear();
        closedSet.Clear();

        startNode.gCost = 0;
        startNode.hCost = CalculateHeuristic(startNode.coords, targetCords);
        startNode.fCost = startNode.gCost + startNode.hCost;

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            openSet.Sort((a, b) => a.fCost.CompareTo(b.fCost));
            currentNode = openSet[0];
            openSet.RemoveAt(0);

            closedSet.Add(currentNode);

            if (currentNode.coords == targetCords)
            {
                return;
            }

            ExploreNeighbors();
        }
    }


    void ExploreNeighbors()
    {
        foreach (Vector2Int direction in searchOrder)
        {
            Vector2Int neighborCords = currentNode.coords + direction;

            if (grid.ContainsKey(neighborCords))
            {
                Tile neighbor = grid[neighborCords];

                if (neighbor.Blocked || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int tentativeGCost = currentNode.gCost + 1;

                if (tentativeGCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = CalculateHeuristic(neighbor.coords, targetCords);
                    neighbor.fCost = neighbor.gCost + neighbor.hCost;
                    neighbor.connectTo = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    int CalculateHeuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    List<Tile> BuildPath()
    {
        List<Tile> path = new List<Tile>();
        Tile currentNode = targetNode;

        if (currentNode.connectTo == null)
        {
            return path;
        }

        path.Add(currentNode);
        currentNode.path = true;

        while (currentNode.connectTo != null)
        {
            currentNode = currentNode.connectTo;
            path.Add(currentNode);
            currentNode.path = true;
        }

        path.Reverse();
        return path;
    }

    public void SetNewDestination(Vector2Int startCoordinates, Vector2Int targetCoordinates)
    {
        if (!grid.ContainsKey(startCoordinates) || !grid.ContainsKey(targetCoordinates))
        {
            return;
        }

        if (grid[startCoordinates].Blocked || grid[targetCoordinates].Blocked)
        {
            return;
        }
        
        startCords = startCoordinates;
        targetCords = targetCoordinates;
        startNode = grid[startCords];
        targetNode = grid[targetCords];
        
        GetNewPath();
    }
}
