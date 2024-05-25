using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform tr;

    public Vector3 ofset;

    private void Update()
    {
        Vector3 cameraTarget = tr.position + ofset;
        transform.position = cameraTarget;
    }


}
