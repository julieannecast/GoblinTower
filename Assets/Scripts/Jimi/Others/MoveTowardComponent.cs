using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardComponent : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    public Vector3 targetPosition;

    [SerializeField] float bulletSpeed;
    [SerializeField] Vector3 bulletRotationPerSecond;
    [SerializeField] Space spaceRotation;

    private void OnEnable()
    {
        targetPosition = target.position + offset;
    }

    private void Update()
    {
        MoveTowardTarget();
    }

    private void MoveTowardTarget()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, bulletSpeed / 100f);
        transform.Rotate(bulletRotationPerSecond * Time.deltaTime, spaceRotation);
    }
}
