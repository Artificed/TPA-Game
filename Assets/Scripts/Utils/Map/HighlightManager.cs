using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HighlightManager: MonoBehaviour
{
    private Vector2Int _currentCoord;
    private Vector2Int _mouseCoord;
    private AStar pathFinder;

    private List<Tile> _highlightedTiles = new List<Tile>();

    public void HighlightPath(Vector2Int startCoord, Tile destinationTile)
    {
        if (!Player.Instance) return;
        if (pathFinder == null) pathFinder = FindObjectOfType<AStar>();
        pathFinder.SetNewDestination(startCoord, destinationTile.coords);
        List<Tile> path = pathFinder.GetNewPath();

        for (int i = 1; i < path.Count; i++)
        {
            path[i].HighlightTile();
            _highlightedTiles.Add(path[i]);
        }
    }
    
    public void ClearHighlightedTiles()
    {
        foreach (var tile in _highlightedTiles)
        {
            tile.UnhighlightTile();
        }
        _highlightedTiles.Clear();
    }
}
