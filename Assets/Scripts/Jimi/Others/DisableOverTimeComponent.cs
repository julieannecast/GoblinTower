using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class DisableOverTimeComponent : MonoBehaviour
{
    [SerializeField] private float timeBeforeDisable = 10f;

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        yield return new WaitForSecondsRealtime(timeBeforeDisable);
        gameObject.SetActive(false);
    }
}