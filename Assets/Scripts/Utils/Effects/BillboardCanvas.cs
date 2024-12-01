using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 targetDirection = transform.position + Camera.main.transform.rotation * Vector3.forward;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection - transform.position, Camera.main.transform.rotation * Vector3.up);
        targetRotation *= Quaternion.Euler(0, 180, 0);
        transform.rotation = targetRotation;
    }
}

