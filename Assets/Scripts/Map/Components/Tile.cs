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
    [SerializeField] private GridManager _gridManager;
    
    public Tile(Vector2Int coords, bool blocked)
    {
        this.coords = coords;
        this.blocked = blocked;
        this.explored = false;
        this.path = false;
        this.gCost = int.MaxValue;
    }

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _tileRenderer = GetComponent<Renderer>();
        _originalColor = _tileRenderer.material.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCoords();
        if(blocked)
        {
            _gridManager.BlockNode(coords);
        }
    }

    private void SetCoords()
    {
        int x = (int)transform.position.x;
        int z = (int)transform.position.z;
    
        coords = new Vector2Int(x / _gridManager.UnityGridSize, z / _gridManager.UnityGridSize);
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
        _tileRenderer.material.color = _highlightColor;
    }
    
    private void OnMouseExit()
    {
        _tileRenderer.material.color = _originalColor;
    }

}
