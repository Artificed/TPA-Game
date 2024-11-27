using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusText : MonoBehaviour
{
    private float _destroyTime = 0.6f;  
    private float _floatSpeed = 0.15f; 

    void Start()
    {
        Destroy(gameObject, _destroyTime);
    }

    void Update()
    {
        transform.position += Vector3.up * _floatSpeed * Time.deltaTime;
    }
}