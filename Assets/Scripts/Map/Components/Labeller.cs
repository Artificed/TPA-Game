using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class Labeller : MonoBehaviour
{
    private TextMeshPro label;
    public Vector2Int coordinates = new Vector2Int();
    private GridManager gridManager;

    private void Awake()
    {
        InitializeComponents();
        UpdateLabel();
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            label.enabled = true;
            UpdateLabel();
            UpdateObjectName();
        }

        HandleLabelToggle();
    }
    
    private void InitializeComponents()
    {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponentInChildren<TextMeshPro>();
        if (label != null)
        {
            label.enabled = false;
        }
    }

    private void UpdateLabel()
    {
        if (gridManager == null || label == null) return;

        coordinates.x = Mathf.RoundToInt(transform.position.x / gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.position.z / gridManager.UnityGridSize);
        label.text = $"{coordinates.x}, {coordinates.y}";
    }
    
    private void UpdateObjectName()
    {
        transform.name = coordinates.ToString();
    }
    
    private void HandleLabelToggle()
    {
        if (Input.GetKeyDown(KeyCode.C) && label != null)
        {
            label.enabled = !label.enabled;
        }
    }
}