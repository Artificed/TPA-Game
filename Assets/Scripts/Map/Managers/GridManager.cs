using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    
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
    private Random _random = new Random();
    
    [SerializeField] private int roomCount = 10;
    [SerializeField] private RoomBuilder _roomBuilder;
    [SerializeField] private DecorationManager decorationManager;
    [SerializeField] private GameObject player;
    
    [SerializeField] private EnemyFactory enemyFactory;

    private Tile _playerTile;
    private int _floor;
    
    private void Start()
    {
        _floor = Player.Instance.Floor;
            
        _rooms = new List<Room>();
        _mst = new List<(Vector2, Vector2)>();
        
        _roomBuilder.Initialize(gridSize, _roomMinSize, _roomMaxSize, tilePrefab, _grid, decorationManager);
        
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
            if (entry.Value == null)  continue;
            entry.Value.connectTo = null;
            entry.Value.explored = false;
            entry.Value.path = false;
        }
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();

        position.x = coordinates.x * unityGridSize;
        position.y = 0.1f;
        position.z = coordinates.y * unityGridSize;

        return position;
    }
    private void CreateGrid()
    {
        GenerateRooms(); 
        GenerateMST(); 
        GenerateHallways(); 
        PlaceInvisibleTiles();
        UpdateRoomEntrances();
        PlaceDecorations();
        InitializePlayer();
        InitializeEnemies();
    }
   
    private void InstantiateTile(Vector2Int coords, GameObject prefab, bool isBlocked, TileType tileType)
    {
        GameObject tileObject = Instantiate(prefab);
        tileObject.transform.position = new Vector3(coords.x, 0f, coords.y);
        tileObject.transform.parent = transform;
        tileObject.name = $"{tileType.ToString()}: {coords.x},{coords.y}";
        
        Tile tile = tileObject.GetComponent<Tile>();
        if (tile != null)
        {
            tile.coords = coords;
            tile.Blocked = isBlocked;
            tile.TileType = tileType;
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
        if (_grid.ContainsKey(position))
        {
            return;
        }

        InstantiateTile(position, tilePrefab, false, TileType.Hallway);
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
                    InstantiateTile(position, invisiblePrefab, true, TileType.Empty);
                }
            }
        }
    }
    
    private void UpdateRoomEntrances()
    {
        foreach (Room room in _rooms)
        {
            List<Vector2Int> entranceCoords = new List<Vector2Int>();

            foreach (Tile tile in room.GetTiles())
            {
                foreach (Vector2Int direction in new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
                {
                    Vector2Int neighborCoords = tile.coords + direction;
                    if (_grid.ContainsKey(neighborCoords) && _grid[neighborCoords].TileType == TileType.Hallway)
                    {
                        entranceCoords.Add(tile.coords);
                        break;
                    }
                }
            }

            foreach (Vector2Int entrance in entranceCoords)
            {
                Tile entranceTile = _grid[entrance];
                entranceTile.TileType = TileType.Entrance;
            }
        }
    }

    private void PlaceDecorations()
    {
        foreach (Room room in _rooms)
        {
            decorationManager.BlockRandomTiles(room);
            decorationManager.GenerateDecorations(room);
        }
    }

    public Tile GetTileFromCoord(Vector2Int coord)
    {
        if (_grid.ContainsKey(coord))
        {
            return _grid[coord];
        }

        return null;
    }
    
    public void InitializePlayer()
    {
        List<Tile> unblockedTiles = new List<Tile>();
        foreach (var tile in _grid.Values)
        {
            if (tile != null && !tile.Blocked)
            {
                unblockedTiles.Add(tile);
            }
        }

        if (unblockedTiles.Count == 0) return;

        Tile randomTile = unblockedTiles[_random.Next(unblockedTiles.Count)];

        _playerTile = randomTile;
        
        Vector3 playerPosition = randomTile.transform.position;
        playerPosition.y = 0.1f;
        player.transform.position = playerPosition;
    }

    public void InitializeEnemies()
    {
        int totalRooms = _rooms.Count;
        int totalEnemies = Mathf.CeilToInt(5 + _floor * 0.2f);
        
        int baseEnemiesPerRoom = totalEnemies / totalRooms;
        int remainingEnemies = totalEnemies % totalRooms;

        for (int i = 0; i < _rooms.Count; i++)
        {
            int roomEnemyCount = baseEnemiesPerRoom;

            if (remainingEnemies > 0)
            {
                roomEnemyCount++;
                remainingEnemies--;
            }

            SpawnEnemiesInRoom(_rooms[i], roomEnemyCount);
        }
    }

    private void SpawnEnemiesInRoom(Room room, int enemyCount)
    {
        List<Tile> unblockedTiles = room.GetTiles().FindAll(tile => !tile.Blocked);
        unblockedTiles.Remove(_playerTile);

        if (unblockedTiles.Count < enemyCount)
        {
            Debug.LogWarning("Not enough unblocked tiles!");
            enemyCount = unblockedTiles.Count;
        }

        HashSet<Tile> usedTiles = new HashSet<Tile>();
        int generatedEnemies = 0;

        while (generatedEnemies < enemyCount)
        {
            int randomIndex = _random.Next(unblockedTiles.Count);
            Tile chosenTile = unblockedTiles[randomIndex];

            if (usedTiles.Contains(chosenTile)) continue;

            usedTiles.Add(chosenTile);

            EnemyType randomType = GetRandomEnemyType();
            
            switch (randomType)
            {
                case EnemyType.Common:
                    enemyFactory.CreateCommonEnemy(chosenTile.coords, _floor);
                    break;

                case EnemyType.Medium:
                    enemyFactory.CreateMediumEnemy(chosenTile.coords, _floor);
                    break;

                case EnemyType.Elite:
                    enemyFactory.CreateEliteEnemy(chosenTile.coords, _floor);
                    break;
            }
            generatedEnemies++;
        }
    }
    
    private EnemyType GetRandomEnemyType()
    {
        float commonWeight = Mathf.Max(100 - (_floor * 1.5f), 20); 
        float mediumWeight = Mathf.Clamp(_floor * 1.2f, 5, 50);    
        float eliteWeight = Mathf.Clamp(_floor * 0.8f - 10, 0, 30);

        float totalWeight = commonWeight + mediumWeight + eliteWeight;

        float randomValue = UnityEngine.Random.Range(0, totalWeight);

        if (randomValue < commonWeight)
        {
            return EnemyType.Common;
        }
        else if (randomValue < commonWeight + mediumWeight)
        {
            return EnemyType.Medium;
        }
        else
        {
            return EnemyType.Elite;
        }
    }
    
    public List<Room> Rooms => _rooms;
}
