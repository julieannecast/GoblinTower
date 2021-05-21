using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveBackAndForthComponent : MonoBehaviour
{
    [SerializeField] Transform startTarget;
    [SerializeField] Transform endTarget;
    [SerializeField] float acceleration;
    [SerializeField] float movementSpeed;
    [SerializeField] float totalAmountOfRotation;
    [SerializeField] float jumpForce;

    private Vector3 desiredPosition;
    private float animationTime;
    private float time;
    private Vector3 rotation;
    private float slowerFactor;
    private void Start()
    {
        rotation = Vector3.zero;
    }

    private void Update()
    {
        slowerFactor = Mathf.Max(1, slowerFactor - Time.deltaTime);
        time = Mathf.PingPong(animationTime * movementSpeed, 1f);
        desiredPosition = Vector3.Lerp(startTarget.position, endTarget.position, time);
        desiredPosition.y = transform.position.y;
        transform.position = desiredPosition;
        rotation.z = totalAmountOfRotation * -time;
        transform.eulerAngles = rotation;
        animationTime += Time.deltaTime / slowerFactor;
    }

    private void OnTriggerEnter(Collider other)
    {
        slowerFactor = 2f;
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
    }
}
