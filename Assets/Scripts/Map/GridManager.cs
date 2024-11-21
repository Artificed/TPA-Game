using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] int unityGridSize;
    
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject invisiblePrefab;
    
    public int UnityGridSize { get { return unityGridSize; } }

    private Dictionary<Vector2Int, Tile> _grid = new Dictionary<Vector2Int, Tile>();
    public Dictionary<Vector2Int, Tile> Grid { get { return _grid; } }
    
    private List<Room> _rooms;
    private Vector2Int _roomMinSize = new Vector2Int(5, 5);
    private Vector2Int _roomMaxSize = new Vector2Int(7,7);
    
    private List<(Vector2, Vector2)> _mst;

    [SerializeField] private int roomCount = 10;
    [SerializeField] private RoomBuilder _roomBuilder;

    private void Awake()
    {
        _rooms = new List<Room>();
        _mst = new List<(Vector2, Vector2)>();
        
        _roomBuilder.Initialize(gridSize, _roomMinSize, _roomMaxSize, tilePrefab, _grid);
        
        CreateGrid();
    }
    
    public void BlockNode(Vector2Int coordinates)
    {
        if (_grid.ContainsKey(coordinates))
        {
            _grid[coordinates].Blocked = true;
        }
    }

    public void ResetNodes()
    {
        foreach (KeyValuePair<Vector2Int, Tile> entry in _grid)
        {
            if (entry.Value == null) 
            {
                Debug.LogWarning($"Tile at {entry.Key} is null. Skipping.");
                continue;
            }

            entry.Value.connectTo = null;
            entry.Value.explored = false;
            entry.Value.path = false;
        }
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();

        position.x = coordinates.x * unityGridSize;
        position.y = 0.5f;
        position.z = coordinates.y * unityGridSize;

        return position;
    }
   private void CreateGrid()
    { 
        GenerateRooms();
        GenerateMST();
        GenerateHallways();
        PlaceInvisibleTiles();
    }
   
    private void InstantiateTile(Vector2Int coords, GameObject prefab, bool isBlocked)
    {
        GameObject tileObject = Instantiate(prefab);
        tileObject.transform.position = new Vector3(coords.x, 0f, coords.y);
        tileObject.transform.parent = transform;
        tileObject.name = isBlocked ? $"Hallway {coords.x},{coords.y}" : $"Tile {coords.x},{coords.y}";

        Tile tile = tileObject.GetComponent<Tile>();
        if (tile != null)
        {
            tile.coords = coords;
            tile.Blocked = isBlocked;
        }

        _grid[coords] = tile;
    }

    private void GenerateRooms()
    {
        _rooms = _roomBuilder.GenerateRooms(roomCount);
    }

    public void GenerateMST()
    {
        Prim prim = new Prim(_rooms);
        _mst = prim.GenerateMST();
    }

    public void GenerateHallways()
    {
        foreach (var edge in _mst)
        {
            Vector2 start = edge.Item1;
            Vector2 end = edge.Item2;

            Vector2Int startCoord = new Vector2Int(Mathf.RoundToInt(start.x), Mathf.RoundToInt(start.y));
            Vector2Int endCoord = new Vector2Int(Mathf.RoundToInt(end.x), Mathf.RoundToInt(end.y));

            CreateLShapedHallway(startCoord, endCoord);
        }
    }

    private void CreateLShapedHallway(Vector2Int start, Vector2Int end)
    {
        int xStep = start.x < end.x ? 1 : -1;
        for (int x = start.x; x != end.x; x += xStep)
        {
            Vector2Int position = new Vector2Int(x, start.y);
            PlaceHallwayTile(position);
        }

        int yStep = start.y < end.y ? 1 : -1;
        for (int y = start.y; y != end.y; y += yStep)
        {
            Vector2Int position = new Vector2Int(end.x, y);
            PlaceHallwayTile(position);
        }
    }
    
    private void PlaceHallwayTile(Vector2Int position)
    {
        if (_grid.ContainsKey(position) && !_grid[position].Blocked)
        {
            return;
        }

        InstantiateTile(position, tilePrefab, false);
    }

    private void PlaceInvisibleTiles()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (!_grid.ContainsKey(position))
                {
                    InstantiateTile(position, invisiblePrefab, true);
                }
            }
        }
    }

    public List<Room> Rooms => _rooms;
}
