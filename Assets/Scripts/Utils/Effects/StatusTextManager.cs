using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusTextManager : MonoBehaviour
{
    [SerializeField] private StatusText statusTextPrefab;
    
    private void Awake()
    {
        SetupPool();
    }

    private void SetupPool()
    {
        ObjectPooler.SetupPool(statusTextPrefab, 5, "statusText");
    }
}