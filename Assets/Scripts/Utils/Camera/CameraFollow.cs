using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; 

    [SerializeField] private CameraShakeEventChannel cameraShakeEventChannel; 

    private Vector3 shakeOffset; 
    private Vector3 originalPosition; 
    
    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + shakeOffset;
        }
    }
    
    private void OnEnable()
    {
        if (cameraShakeEventChannel != null)
        {
            cameraShakeEventChannel.ShakeCameraEvent += StartCameraShake;
        }
    }

    private void OnDisable()
    {
        if (cameraShakeEventChannel != null)
        {
            cameraShakeEventChannel.ShakeCameraEvent -= StartCameraShake;
        }
    }
    private void StartCameraShake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCamera(duration, magnitude));
    }

    private IEnumerator ShakeCamera(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        shakeOffset = Vector3.zero; 

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            shakeOffset = new Vector3(x, y, 0f); 

            elapsed += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
    }
}

