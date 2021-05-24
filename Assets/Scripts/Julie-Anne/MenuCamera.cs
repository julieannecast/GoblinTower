using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    void Update()
    {
        transform.RotateAround(new Vector3(), Vector3.up, 60 * Time.deltaTime);
    }
}
