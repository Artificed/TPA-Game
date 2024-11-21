using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public float StartX { get; }
    public float StartY { get; }
    public float EndX { get; }
    public float EndY { get; }
    public Vector2 Center { get; }

    private readonly List<Tile> _tiles = new();
    private readonly bool _isEntranceCoord;

    public Room(float startX, float startY, float endX, float endY)
    {
        StartX = startX;
        StartY = startY;
        EndX = endX;
        EndY = endY;
        Center = new Vector2((StartX + EndX) / 2, (StartY + EndY) / 2);
        
    }

    public void AddTile(Tile tile) => _tiles.Add(tile);

    public List<Tile> GetTiles() => new(_tiles);
}