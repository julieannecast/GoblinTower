using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardComponent : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed;

    private Vector3 targetPosition;

    private void OnEnable()
    {
        targetPosition = target.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
    }
}
