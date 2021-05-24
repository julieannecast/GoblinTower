using System.Collections;
using UnityEngine;

public class CanonBehavior : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float aggroRange;
    [SerializeField] float rotationSpeed;
    [SerializeField] bool lockRotation;
    [SerializeField] ObjectEmitterComponent emitter;

    private Vector3 targetPosition;

    private CourbeBezier curve;
    [SerializeField] float curveMaxHeight;

    private LineRenderer lineRenderer;
    private const int PointsCount = 10;
    private Vector3[] points;

    [SerializeField] new ParticleSystem particleSystem;
    private Vector3 particleSystemPosition;
    private Quaternion particleSystemRotation;

    private bool isShooting;
    private bool isReady;

    private const float TrajectoryIndicatorFadeInSpeed = 0.5f;
    private const float TrajectoryIndicatorFadeOutSpeed = 0.5f;

    private void Start()
    {
        points = new Vector3[PointsCount];
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = PointsCount;

        isReady = true;

        StartCoroutine(Shoot());
    }

    private void Update()
    {
        UpdateParticleSystem();
        CheckTargetDistance(target.position, out isShooting);
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            if (isShooting && isReady)
            {
                isReady = false;

                SetTargetPosition();
                StartCoroutine(TrajectoryIndicatorFadeIn());

                //Le canon se tourne en direction du nouveau target
                float time = 0;
                while (time < 0.9f)
                {
                    LookTarget();
                    SetTrajectoryIndicator();
                    time = Mathf.Lerp(time, 1f, rotationSpeed * Time.deltaTime);
                    yield return null;
                }

                SetTrajectoryCurve();

                emitter.CanEmit(true); //Maintenant l'émetteur peut émettre
            }

            yield return null;
        }
    }

    //Fonction appelée après l'émission d'un projectile
    public void PostEmit(GameObject recycledObject)
    {
        recycledObject.GetComponent<MoveArcComponent>().SetCurve(curve);

        ResetParticleSystem();
        particleSystem.Play();

        StartCoroutine(TrajectoryIndicatorFadeOut());
        emitter.CanEmit(false);
    }

    private void ResetParticleSystem()
    {
        particleSystemPosition = emitter.exitPoint.position;
        particleSystemRotation = emitter.exitPoint.rotation;
    }

    private void UpdateParticleSystem()
    {
        particleSystem.transform.position = particleSystemPosition;
        particleSystem.transform.rotation = particleSystemRotation;
    }

    private void GetTrajectoryReferencePoints(out Vector3 a, out Vector3 b, out Vector3 c)
    {
        Vector3 m = Vector3.Lerp(targetPosition, emitter.exitPoint.position, 0.5f);
        float y = Mathf.Max(emitter.exitPoint.position.y, targetPosition.y) + curveMaxHeight;
        Vector3 point = new Vector3(m.x, y, m.z);
        a = emitter.exitPoint.position;
        b = point;
        c = targetPosition;
    }

    private void SetTrajectoryCurve()
    {
        GetTrajectoryReferencePoints(out Vector3 a, out Vector3 b, out Vector3 c);
        curve = new CourbeBezier(a, b, c);
    }

    private void SetTrajectoryIndicator()
    {
        GetTrajectoryReferencePoints(out Vector3 a, out Vector3 b, out Vector3 c);

        float time;
        for (int i = 1; i < PointsCount + 1; i++)
        {
            time = i / (float)PointsCount;
            points[i - 1] = CourbeBezier.Evaluer(a, b, c, time); 
        }

        lineRenderer.SetPositions(points);
    }

    IEnumerator TrajectoryIndicatorFadeIn()
    {
        float elapsedTime = 0;

        Gradient gradient = new Gradient();

        while (elapsedTime < TrajectoryIndicatorFadeInSpeed)
        {
            gradient.SetKeys(lineRenderer.colorGradient.colorKeys,
                 new GradientAlphaKey[] { new GradientAlphaKey(elapsedTime / TrajectoryIndicatorFadeInSpeed, 1f) }
             );
            lineRenderer.colorGradient = gradient;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator TrajectoryIndicatorFadeOut()
    {
        float elapsedTime = 0;

        Gradient gradient = new Gradient();

        while (elapsedTime < TrajectoryIndicatorFadeOutSpeed)
        {
            gradient.SetKeys (lineRenderer.colorGradient.colorKeys,
                 new GradientAlphaKey[] { new GradientAlphaKey(1 - (elapsedTime / TrajectoryIndicatorFadeOutSpeed), 1f) }
             );
            lineRenderer.colorGradient = gradient;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        isReady = true;
    }

    private void SetTargetPosition()
    {
        targetPosition = target.position + offset;
    }

    private void CheckTargetDistance(Vector3 target, out bool isShooting) =>
        isShooting = Vector3.Distance(transform.position, target) <= aggroRange;

    private Vector3 GetLookRotation() => 
        Quaternion.LookRotation(targetPosition - transform.parent.position).eulerAngles;

    private void LookTarget()
    {
        if (lockRotation) return;
        Vector3 rotation = GetLookRotation();
        rotation.x = 0;
        rotation.z = 0;
        transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, Quaternion.Euler(rotation), rotationSpeed * Time.deltaTime);
    }
}
