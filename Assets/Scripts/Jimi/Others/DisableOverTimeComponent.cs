using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class DisableOverTimeComponent : MonoBehaviour
{
    public float timeBeforeDisable = 10f;

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    public void Cancel()
    {
        StopCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        yield return new WaitForSecondsRealtime(timeBeforeDisable);
        gameObject.SetActive(false);
    }
}