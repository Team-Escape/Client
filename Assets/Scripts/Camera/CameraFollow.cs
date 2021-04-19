using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform SetTarget { set { target = value; } }
    [SerializeField]
    Transform target = null;
    [SerializeField]
    float smoothSpeed = 1f;
    [SerializeField]
    Vector3 offset = Vector3.zero;

    Camera cam;
    Rigidbody2D rb;

    private void Awake()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        Vector2 desirePosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desirePosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -10);
    }
}
