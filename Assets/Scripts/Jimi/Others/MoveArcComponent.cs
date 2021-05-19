using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArcComponent : MonoBehaviour
{
    [SerializeField] Transform target;

    private Vector3 targetPosition;
    public float privateRotation;
    public float dragDown;


    private void OnEnable()
    {
        targetPosition = target.position;
        StartCoroutine(CannonballMovement());
    }

    IEnumerator CannonballMovement()
    {
        yield return new WaitForSeconds(0f);

        float target_Distance = Vector3.Distance(transform.position, targetPosition);

        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * privateRotation * Mathf.Deg2Rad) / dragDown);

        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(privateRotation * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(privateRotation * Mathf.Deg2Rad);

        float flightDuration = target_Distance / Vx;

        transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (dragDown * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }
}
