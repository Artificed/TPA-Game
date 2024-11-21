using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DecorationManager: MonoBehaviour
{
    [SerializeField] private List<GameObject> decorationPrefabs;
    [SerializeField] private GameObject groupedDecorationPrefab;
    [SerializeField] private float bufferDistance;

    private Random _random = new Random();
    public void GenerateDecorations(Room room)
    {
        List<Tile> blockedTiles = room.GetTiles().FindAll(tile => tile.Blocked);
        List<Vector3> placedPositions = new List<Vector3>();
        
        foreach (Tile tile in blockedTiles)
        {
            Vector3 position = tile.transform.position;

            position.x += (float)(_random.NextDouble() * 0.3 - 0.15);
            position.z += (float)(_random.NextDouble() * 0.3 - 0.15);

            // if (!IsPositionValid(position, placedPositions)) continue;

            GameObject prefab = ChooseRandomDecoration();
            if (prefab == null)
            {
                Debug.LogError("No valid decoration prefab found. Skipping decoration placement.");
                continue;
            }

            Quaternion rotation = Quaternion.Euler(0, _random.Next(0, 4) * 90, 0); // Random rotation
            GameObject decoration = Instantiate(prefab, position, rotation);
            decoration.tag = "Decoration";

            placedPositions.Add(position);
        }
    }
    
    // private bool IsPositionValid(Vector3 position, List<Vector3> placedPositions)
    // {
    //     foreach (Vector3 placedPosition in placedPositions)
    //     {
    //         if (Vector3.Distance(position, placedPosition) < bufferDistance)
    //         {
    //             return false;
    //         }
    //     }
    //     return true;
    // }
    
    public GameObject ChooseRandomDecoration()
    {
        if (decorationPrefabs == null || decorationPrefabs.Count == 0)
        {
            Debug.LogWarning("No decoration prefabs assigned.");
            return null;
        }

        if (_random.NextDouble() > 0.8f && groupedDecorationPrefab != null)
        {
            return groupedDecorationPrefab;
        }

        return decorationPrefabs[_random.Next(decorationPrefabs.Count)];
    }
}
