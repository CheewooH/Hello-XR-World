using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        Vector3 targetPosition = new Vector3(cameraTransform.position.x,
                                             transform.position.y,
                                             cameraTransform.position.z);
        transform.LookAt(targetPosition);
    }
}

