using UnityEngine;
using System.Collections;

public class DisableOverTimeComponent : MonoBehaviour
{
    [SerializeField] float timeBeforeDisable = 10f;

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