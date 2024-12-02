using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusText : MonoBehaviour
{
    private float _destroyTime = 0.6f;  
    private float _floatSpeed = 0.15f; 

    private string _poolKey = "statusText";
    private float _elapsedTime;
    
    void OnEnable()
    {
        _elapsedTime = 0f; 
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;

        transform.position += Vector3.up * _floatSpeed * Time.deltaTime;

        if (_elapsedTime >= _destroyTime)
        {
            ObjectPooler.EnqueueObject(this, _poolKey);
            transform.SetParent(null);
        }
    }
}