using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/CameraShakeEventChannel", fileName = "CameraShakeEventChannel")]
public class CameraShakeEventChannel : ScriptableObject
{
    public UnityAction<float, float> ShakeCameraEvent;

    public void RaiseEvent(float duration, float magnitude)
    {
        ShakeCameraEvent.Invoke(duration, magnitude);
    }
}
