using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomBuilder: MonoBehaviour
{
    private Vector2Int gridSize;
    private Vector2Int roomMinSize;
    private Vector2Int roomMaxSize;
    private GameObject tilePrefab;
    private Dictionary<Vector2Int, Tile> grid;
    private List<Room> rooms;
    private System.Random random;
    private DecorationManager _decorationManager;
    
    public void Initialize(Vector2Int gridSize, Vector2Int roomMinSize, Vector2Int roomMaxSize,
        GameObject tilePrefab, Dictionary<Vector2Int, Tile> grid, DecorationManager decorationManager)
    {
        this.gridSize = gridSize;
        this.roomMinSize = roomMinSize;
        this.roomMaxSize = roomMaxSize;
        this.tilePrefab = tilePrefab;
        this.grid = grid;
        this.rooms = new List<Room>();
        this.random = new System.Random();
        _decorationManager = decorationManager;
    }

    public List<Room> GenerateRooms(int roomCount)
    {
        int generatedRoomCount = 0;

        while (generatedRoomCount < roomCount)
        {
            int randStartX = random.Next(0, gridSize.x - roomMaxSize.x);
            int randStartY = random.Next(0, gridSize.y - roomMaxSize.y);

            if (generatedRoomCount == 0)
            {
                randStartX = 0;
                randStartY = 0;
            }

            int randSizeX = random.Next(roomMinSize.x, roomMaxSize.x + 1);
            int randSizeY = random.Next(roomMinSize.y, roomMaxSize.y + 1);
            int endX = randStartX + randSizeX;
            int endY = randStartY + randSizeY;

            int totalTiles = randSizeX * randSizeY;
            
            int minDecorations = Mathf.FloorToInt(totalTiles * 0.2f);
            int maxDecorations = Mathf.CeilToInt(totalTiles * 0.3f);
            
            int decorationCount = random.Next(minDecorations, maxDecorations + 1);

            Room newRoom = new Room(randStartX, randStartY, endX - 1, endY - 1);

            if (CheckOverlap(newRoom, rooms))
                continue;

            generatedRoomCount++;
            CreateRoomTiles(newRoom, decorationCount);
            rooms.Add(newRoom);
            
            if (_decorationManager != null)
            {
                _decorationManager.GenerateDecorations(newRoom);
            }
        }

        return rooms;
    }
    
    private void CreateRoomTiles(Room newRoom, int decorationCount)
    {
        HashSet<Vector2Int> blockedTiles = new HashSet<Vector2Int>();
        List<Vector2Int> potentialTiles = new List<Vector2Int>();
        List<Vector2Int> entranceCoords = GetEntranceCoordinates(newRoom);
        
        CollectPotentialTiles(newRoom, entranceCoords, potentialTiles);
        BlockTilesForDecoration(decorationCount, potentialTiles, entranceCoords, blockedTiles);
        CreateOrUpdateRoomTiles(newRoom, blockedTiles);
    }
    
    public List<Vector2Int> GetEntranceCoordinates(Room newRoom)
    {
        List<Vector2Int> entranceCoords = new List<Vector2Int>();

        for (int x = (int)newRoom.StartX; x <= newRoom.EndX; x++)
        {
            for (int y = (int)newRoom.StartY; y <= newRoom.EndY; y++)
            {
                Vector2Int coords = new Vector2Int(x, y);
                if (!grid.ContainsKey(coords)) continue;

                Tile currentTile = grid[coords];
                if (currentTile.TileType != TileType.Room) continue;

                foreach (Vector2Int direction in new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
                {
                    Vector2Int neighborCoords = coords + direction;
                    if (grid.ContainsKey(neighborCoords) && grid[neighborCoords].TileType == TileType.Hallway)
                    {
                        entranceCoords.Add(coords);
                        break;
                    }
                }
            }
        }

        return entranceCoords;
    }
    
    private void CollectPotentialTiles(Room newRoom, List<Vector2Int> entranceCoords, List<Vector2Int> potentialTiles)
    {
        for (int x = (int)newRoom.StartX; x <= newRoom.EndX; x++)
        {
            for (int y = (int)newRoom.StartY; y <= newRoom.EndY; y++)
            {
                Vector2Int coords = new Vector2Int(x, y);
                if (!entranceCoords.Contains(coords))
                {
                    potentialTiles.Add(coords);
                }
            }
        }
    }
    
    private void BlockTilesForDecoration(int decorationCount, List<Vector2Int> potentialTiles, List<Vector2Int> entranceCoords, HashSet<Vector2Int> blockedTiles)
    {
        System.Random random = new System.Random();
        int maxAttempts = potentialTiles.Count * 10;
        int attempts = 0;

        while (blockedTiles.Count < decorationCount && potentialTiles.Count > 0)
        {
            if (attempts > maxAttempts) break;

            Vector2Int candidate = potentialTiles[random.Next(potentialTiles.Count)];

            bool isValid = true;
            foreach (Vector2Int direction in new[]
                     {
                         Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
                         new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1)
                     })
            {
                if (blockedTiles.Contains(candidate + direction) || entranceCoords.Contains(candidate))
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                blockedTiles.Add(candidate);
                potentialTiles.Remove(candidate);
            }

            attempts++;
        }

        if (blockedTiles.Count < decorationCount)
        {
            Debug.LogWarning($"Not enough tiles to block {decorationCount} tiles. Blocked {blockedTiles.Count} instead.");
        }
    }
    
    private void CreateOrUpdateRoomTiles(Room newRoom, HashSet<Vector2Int> blockedTiles)
    {
        for (int x = (int)newRoom.StartX; x <= newRoom.EndX; x++)
        {
            for (int y = (int)newRoom.StartY; y <= newRoom.EndY; y++)
            {
                Vector2Int coords = new Vector2Int(x, y);

                if (!grid.ContainsKey(coords))
                {
                    GameObject tileObject = Instantiate(tilePrefab);
                    tileObject.transform.position = new Vector3(coords.x, 0f, coords.y);
                    tileObject.name = $"Tile {coords.x},{coords.y}";

                    Tile tileScript = tileObject.GetComponent<Tile>();
                    if (tileScript != null)
                    {
                        tileScript.coords = coords;
                        tileScript.Blocked = blockedTiles.Contains(coords);
                        tileScript.TileType = TileType.Room;
                        grid[coords] = tileScript;
                        newRoom.AddTile(tileScript);
                    }
                }
                else
                {
                    // Update existing tile
                    Tile existingTile = grid[coords];
                    existingTile.Blocked = blockedTiles.Contains(coords);
                    existingTile.TileType = TileType.Room;
                    newRoom.AddTile(existingTile);
                }
            }
        }
    }
    
    private bool CheckOverlap(Room newRoom, List<Room> existingRooms)
    {
        foreach (Room room in existingRooms)
        {
            if (newRoom.StartX - 2 > room.EndX ||
                newRoom.EndX + 2 < room.StartX ||
                newRoom.StartY - 2 > room.EndY ||
                newRoom.EndY + 2 < room.StartY)
            {
                continue;
            }
            return true;
        }
        return false;
    }
}
