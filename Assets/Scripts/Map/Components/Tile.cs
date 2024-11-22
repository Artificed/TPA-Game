using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    [SerializeField] bool blocked;
    [SerializeField] public Vector2Int coords;
    
    public bool explored;
    public bool path;
    public int gCost;
    public int hCost;
    public int fCost;
    public Tile connectTo;

    private Color _originalColor;
    private Color _highlightColor;
    
    private Renderer _tileRenderer;
    [SerializeField] private TileType tileType;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private HighlightManager highlightManager;
    
    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        _tileRenderer = GetComponentInChildren<Renderer>();
        highlightManager = FindObjectOfType<HighlightManager>();
        _originalColor = _tileRenderer.material.color;
        _highlightColor = new Color(1.0f, 1.0f, 1.0f, 0.2f);;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCoords();
        if(blocked)
        {
            gridManager.BlockNode(coords);
        }
    }

    private void SetCoords()
    {
        int x = (int)transform.position.x;
        int z = (int)transform.position.z;
    
        coords = new Vector2Int(x / gridManager.UnityGridSize, z / gridManager.UnityGridSize);
    }

    public bool Blocked
    {
        get => blocked;
        set => blocked = value;
    }
    
    public void Initialize(Vector2Int coords, bool blocked)
    {
        this.coords = coords;
        this.blocked = blocked;
        this.explored = false;
        this.path = false;
        this.gCost = int.MaxValue;
    }

    private void OnMouseEnter()
    {
        int startX = (int) PlayerStateMachine.Instance.Unit.position.x;
        int startY = (int) PlayerStateMachine.Instance.Unit.position.z;
        highlightManager.HighlightPath(new Vector2Int(startX, startY), this);
    }
    
    private void OnMouseExit()
    {
        highlightManager.ClearHighlightedTiles();
    }

    public void HighlightTile()
    {
        _tileRenderer.material.color = _highlightColor;
    }
    
    public void UnhighlightTile()
    {
        _tileRenderer.material.color = _originalColor;
    }

    public TileType TileType
    {
        get => tileType;
        set => tileType = value;
    }
}
