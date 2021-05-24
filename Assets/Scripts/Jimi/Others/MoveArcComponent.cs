using System.Collections;
using UnityEngine;

/*
 * Note: Une courbe de bezier doit être assigné 'SetCurve' 
 *       avant le début de la coroutine 'ArcMovement'.
 */

[RequireComponent(typeof(Rigidbody))]
public class MoveArcComponent : MonoBehaviour
{
    [SerializeField] float bulletSpeed;

    private CourbeBezier curve;
    private float flightDuration;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        flightDuration = 0;
    }

    private void OnEnable()
    {
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

        rb.useGravity = true;
    }

    public void SetCurve(CourbeBezier curve)
    {
        this.curve = curve;
    }
}
