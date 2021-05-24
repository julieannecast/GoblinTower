using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MoveTowardComponent))]
[RequireComponent(typeof(SpiningComponent))]
[RequireComponent(typeof(ScalePulsateComponent))]
[RequireComponent(typeof(DisableOverTimeComponent))]
public class ExplosiveBulletBehavior : MonoBehaviour, IPoolable
{
    public ObjectPoolComponent associatedPool { get; set; }

    private MoveTowardComponent moveTowardComponent;
    private SpiningComponent spiningComponent;
    private ScalePulsateComponent scalePulsateComponent;
    private DisableOverTimeComponent disableOverTimeComponent;

    [SerializeField] float distanceMinToDetonate;
    [SerializeField] float timeBeforeExplosion; //Secondes
    private bool detonated;
    [SerializeField] ParticleSystem particleSys;

    [SerializeField] GameObject projectilePieces;

    private void Awake()
    {
        moveTowardComponent = GetComponent<MoveTowardComponent>();
        spiningComponent = GetComponent<SpiningComponent>();
        scalePulsateComponent = GetComponent<ScalePulsateComponent>();
        disableOverTimeComponent = GetComponent<DisableOverTimeComponent>();
    }

    private void OnEnable()
    {
        detonated = false;
        projectilePieces.SetActive(true);
    }

    private void OnDisable()
    {
        disableOverTimeComponent.Cancel();
        associatedPool.PutObject(gameObject);
    }

    private void Update()
    {
        if (!detonated && CloseEnoughToDetonate())
        {
            detonated = true;
            scalePulsateComponent.StartPulsing();
            StartCoroutine(TicTac());
        }
    }

    private bool CloseEnoughToDetonate() =>
        moveTowardComponent.DistanceFromTarget() <= distanceMinToDetonate;

    private IEnumerator TicTac()
    {
        float elaspedTime = 0;

        while (elaspedTime < timeBeforeExplosion)
        {
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        Explode();

        yield return new WaitForSecondsRealtime(particleSys.main.duration);

        Die();
    }

    private void Explode()
    {
        spiningComponent.StopSpining();
        particleSys.Play();
        projectilePieces.SetActive(false);
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
