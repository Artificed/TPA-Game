using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DecorationManager: MonoBehaviour
{
    [SerializeField] private List<GameObject> decorationPrefabs;
    [SerializeField] private GameObject groupedDecorationPrefab;
    [SerializeField] private float bufferDistance;
    [SerializeField] private GridManager gridManager;
    
    private List<Vector2Int> _decoratedTiles = new List<Vector2Int>();
    private Random _random = new Random();

    public void BlockRandomTiles(Room room)
    {
        List<Tile> tiles = room.GetTiles();
        List<Vector2Int> blockedTileCoords = new List<Vector2Int>();
        List<Vector2Int> entranceCoords = new List<Vector2Int>();

        int totalTiles = tiles.Count;

        int minDecorations = Mathf.FloorToInt(totalTiles * 0.2f);
        int maxDecorations = Mathf.CeilToInt(totalTiles * 0.3f);
        int decorationCount = _random.Next(minDecorations, maxDecorations + 1);

        int maxAttempts = totalTiles * 2; 
        int attempts = 0;

        foreach (Tile tile in tiles)
        {
            foreach (Vector2Int direction in new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighborCoords = tile.coords + direction;
                Tile adjacentTile = gridManager.GetTileFromCoord(neighborCoords);

                if (adjacentTile != null && adjacentTile.TileType == TileType.Hallway)
                {
                    entranceCoords.Add(tile.coords);
                    break;
                }
            }
        }

        while (blockedTileCoords.Count < decorationCount && attempts < maxAttempts)
        {
            Tile candidateTile = tiles[_random.Next(tiles.Count)];
            Vector2Int candidateCoords = candidateTile.coords;

            bool isValid = true;
            foreach (Vector2Int direction in new[]
                     {
                         Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
                         new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1)
                     })
            {
                if (blockedTileCoords.Contains(candidateCoords + direction) || entranceCoords.Contains(candidateCoords))
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                blockedTileCoords.Add(candidateCoords);
                candidateTile.Blocked = true; 
                tiles.Remove(candidateTile);  
            }

            attempts++;
        }

        // if (blockedTileCoords.Count < decorationCount)
        // {
        //     Debug.LogWarning($"Could not block enough tiles. Blocked {blockedTileCoords.Count} out of {decorationCount} requested.");
        // }
    }

    public void GenerateDecorations(Room room)
    {
        List<Tile> blockedTiles = room.GetTiles().FindAll(tile => tile.Blocked);
        List<Vector3> placedPositions = new List<Vector3>();
        GameObject mapParent = GameObject.Find("Decoration"); 
        
        foreach (Tile tile in blockedTiles)
        {
            Vector3 position = tile.transform.position;

            position.y += 0.2f;
            
            GameObject prefab = ChooseRandomDecoration();
            if (prefab == null) continue;
            if (prefab.name.Equals("crate"))
            {
                position.y += 0.35f;
            }

            Quaternion rotation = Quaternion.Euler(0, _random.Next(0, 4) * 90, 0);
            GameObject decoration = Instantiate(prefab, position, rotation);
            decoration.tag = "Decoration";
            decoration.transform.SetParent(mapParent.transform, false);

            placedPositions.Add(position);
        }
    }
    
    public GameObject ChooseRandomDecoration()
    {
        if (decorationPrefabs == null || decorationPrefabs.Count == 0)
        {
            return null;
        }

        if (_random.NextDouble() > 0.8f && groupedDecorationPrefab != null)
        {
            return groupedDecorationPrefab;
        }

        return decorationPrefabs[_random.Next(decorationPrefabs.Count)];
    }
}
