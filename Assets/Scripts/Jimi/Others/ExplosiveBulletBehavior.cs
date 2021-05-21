using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveTowardComponent))]
[RequireComponent(typeof(ScalePulsateComponent))]
[RequireComponent(typeof(DisableOverTimeComponent))]
public class ExplosiveBulletBehavior : MonoBehaviour, IPoolable
{
    public ObjectPoolComponent associatedPool { get; set; }

    private MoveTowardComponent moveTowardComponent;
    private ScalePulsateComponent scalePulsateComponent;
    private DisableOverTimeComponent disableOverTimeComponent;

    [SerializeField] float distanceMinToDetonate;
    [SerializeField] float timeBeforeExplosion;
    private bool detonated;
    private ParticleSystem particleSys;

    [SerializeField] GameObject skin;

    private void Awake()
    {
        moveTowardComponent = GetComponent<MoveTowardComponent>();
        scalePulsateComponent = GetComponent<ScalePulsateComponent>();
        disableOverTimeComponent = GetComponent<DisableOverTimeComponent>();
        particleSys = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        detonated = false;
        skin.SetActive(true);
    }

    private void OnDisable()
    {
        associatedPool.PutObject(gameObject);
    }

    private void Update()
    {
        if (!detonated && Vector3.Distance(transform.position, moveTowardComponent.targetPosition) <= distanceMinToDetonate)
        {
            detonated = true;
            scalePulsateComponent.StartPulsing();
            StartCoroutine(TicTac());
        }
    }

    private IEnumerator TicTac()
    {
        float elaspedTime = 0;

        while (elaspedTime < timeBeforeExplosion)
        {
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        particleSys.Play();
        skin.SetActive(false);
        yield return new WaitForSecondsRealtime(particleSys.main.duration);

        Explode();
    }

    private void Explode()
    {
        disableOverTimeComponent.Cancel();
        gameObject.SetActive(false);
    }
}
