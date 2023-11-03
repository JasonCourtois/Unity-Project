using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 offset;

    public float smoothing;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    void Start()
    {
        offset = new Vector3(0, 0, transform.position.z - target.position.z);

        Vector3 targetPosition = target.position + offset;
        targetPosition.x = Mathf.Clamp(target.position.x, minPosition.x, maxPosition.x);
        targetPosition.y = Mathf.Clamp(target.position.y, minPosition.y, maxPosition.y);

        transform.position = targetPosition;
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        targetPosition.x = Mathf.Clamp(target.position.x, minPosition.x, maxPosition.x);
        targetPosition.y = Mathf.Clamp(target.position.y, minPosition.y, maxPosition.y);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
    }
}
