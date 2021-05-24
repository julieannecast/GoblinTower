using System.Collections;
using UnityEngine;

public class ScalePulsateComponent : MonoBehaviour
{
    [SerializeField] float maxDeltaScale;
    [SerializeField] float stepPerSecond;

    private Vector3 scaleOriginal;
    private Vector3 transformations;
    private float maxX, maxY, maxZ;
    private float minX, minY, minZ;
    private int facteur = 1;

    public bool isPulsing;

    private void Awake()
    {
        scaleOriginal = transform.localScale;
        transformations = transform.localScale;
        minX = scaleOriginal.x;
        minY = scaleOriginal.y;
        minZ = scaleOriginal.z;
        maxX = scaleOriginal.x + maxDeltaScale;
        maxY = scaleOriginal.y + maxDeltaScale;
        maxZ = scaleOriginal.z + maxDeltaScale;
    }

    private void OnEnable()
    {
        facteur = 1;
        transform.localScale = scaleOriginal;
        transformations = scaleOriginal;
        isPulsing = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void StartPulsing()
    {
        isPulsing = true;
        StartCoroutine(Pulsate());
    }

    public void StopPulsing()
    {
        isPulsing = false;
    }

    private IEnumerator Pulsate()
    {
        yield return null;

        while (isPulsing)
        {
            transformations.x = Mathf.Clamp(transformations.x + facteur * stepPerSecond * Time.deltaTime, minX, maxX);
            transformations.y = Mathf.Clamp(transformations.y + facteur * stepPerSecond * Time.deltaTime, minY, maxY);
            transformations.z = Mathf.Clamp(transformations.z + facteur * stepPerSecond * Time.deltaTime, minZ, maxZ);
            transform.localScale = transformations;

            if (transformations.x >= maxX)
                facteur = -1;
            else if (transformations.x <= scaleOriginal.x)
                facteur = 1;

            yield return null;
        }
    }
}
