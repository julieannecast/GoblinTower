using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMotionComponent : MonoBehaviour
{
    [SerializeField] float amplitude;
    [SerializeField] float radStepPerFrame;
    [SerializeField] Space space;

    float rad;
    private void Update()
    {
        transform.Translate(0, (amplitude * Mathf.Sin(rad)), 0, space);
        rad += radStepPerFrame * Time.deltaTime;
    }
}
