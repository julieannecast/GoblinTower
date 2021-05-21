using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveArcComponent : MonoBehaviour
{
    [SerializeField] CanonBehavior canon;
    [SerializeField] float bulletSpeed;

    private float flightDuration;
    private Vector3 targetPosition;
    private CourbeBezier curve;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        flightDuration = 0;
    }

    private void OnEnable()
    {
        targetPosition = canon.targetPosition;
        curve = canon.curve;
        flightDuration = 0;
        StartCoroutine(ArcMovement());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            StopAllCoroutines();
            rb.useGravity = true;
        }
    }

    IEnumerator ArcMovement()
    {
        yield return null;
        Quaternion startRotation = transform.rotation;

        float rx = Random.Range(0, 360);
        float ry = Random.Range(0, 360);
        float rz = Random.Range(0, 360);

        while (flightDuration < 1f)
        {
            transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(new Vector3(rx, ry, rz)), flightDuration);
            transform.position = curve.Evaluer(flightDuration);
            flightDuration += Time.deltaTime * bulletSpeed;
            yield return null;
        }

        transform.position = targetPosition;
        rb.useGravity = true;
    }
}
