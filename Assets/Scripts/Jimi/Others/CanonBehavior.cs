using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBehavior : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float aggroRange; //Distance à laquelle le canon peut voir le target
    [SerializeField] Transform barrel;
    [SerializeField] float barrelMaxRecoil; //Quantité de recoil lors d'un tir
    [SerializeField] Transform exit; //Point de sortie des projectiles
    [SerializeField] float cooldown; //Temps entre chaque tir
    [SerializeField] float rotationSpeed;
    [SerializeField] ObjectPoolComponent bulletObjectPool;

    private bool isShooting;
    private Vector3 barrelStartPosition;
    private float currentRecoil;

    public CourbeBezier curve; //Aussi une référence pour les projectiles
    [SerializeField] float curveMaxHeight; //Hauteur maximum de la trajectoire d'un projectile selon le target

    public Vector3 targetPosition; //Aussi une référence pour les projectiles

    private LineRenderer lineRenderer;
    private int pointsCount = 10;
    private Vector3[] points;

    private void Start()
    {
        barrelStartPosition = barrel.position;

        points = new Vector3[pointsCount];
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointsCount;

        StartCoroutine(Shoot());
    }

    private void Update()
    {
        isShooting = TargetIsCloseEnough(target.position);

        currentRecoil = Mathf.Max(0, currentRecoil - Time.deltaTime * (1 / cooldown));
        barrel.position = barrelStartPosition + barrel.up * -currentRecoil;
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            if (isShooting)
            {
                SetTargetPosition();
                CalculateTrajectory();
                SetTrajectoryIndicator();
                SetTrajectoryIndicatorColor();

                //Le canon se tourne en direction du nouveau target
                float time = 0;
                while (time < 1f)
                {
                    LookTarget();

                    time += Time.deltaTime * rotationSpeed;
                    yield return null;
                }

                //On récupère un projectile dans le pool
                try
                {
                    var recycledBullet = bulletObjectPool.GetObject();
                    ResetBullet(recycledBullet);
                    currentRecoil = barrelMaxRecoil;
                }
                catch { }
            }

            yield return StartCoroutine(TrajectoryIndicatorFadeOut());

            yield return new WaitForSecondsRealtime(cooldown);
        }
    }

    private bool TargetIsCloseEnough(Vector3 target) =>
        Vector3.Distance(transform.position, target) <= aggroRange;

    private void SetTargetPosition()
    {
        targetPosition = target.position + offset;
    }

    private void ResetBullet(GameObject recycledBullet)
    {
        recycledBullet.transform.position = exit.position;
        recycledBullet.transform.rotation = exit.rotation;
        recycledBullet.SetActive(true);
    }

    private void CalculateTrajectory()
    {
        Vector3 m = Vector3.Lerp(targetPosition, exit.position, 0.5f);
        float y = Mathf.Max(exit.position.y, targetPosition.y) + curveMaxHeight;
        Vector3 point = new Vector3(m.x, y, m.z);
        curve = new CourbeBezier(exit.position, point, targetPosition);
    }

    private void SetTrajectoryIndicator()
    {
        float time;
        for (int i = 1; i < pointsCount + 1; i++)
        {
            time = i / (float)pointsCount;
            points[i - 1] = curve.Evaluer(time);
        }

        lineRenderer.SetPositions(points);
    }

    private void SetTrajectoryIndicatorColor()
    {
        Color color = new Color(255f, 255f, 255f);
        lineRenderer.materials[0].SetColor("_TintColor", color);
    }

    IEnumerator TrajectoryIndicatorFadeOut()
    {
        float fadeOutSpeed = 2f;
        float elapsedTime = 0;

        while (elapsedTime < fadeOutSpeed)
        {
            Color color = Color.Lerp(new Color(0.5f, 0.5f, 0.5f, 0.5f), new Color(0f, 0f, 0f, 0f), elapsedTime);
            lineRenderer.materials[0].SetColor("_TintColor", color);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    private Vector3 GetRotation() => 
        Quaternion.LookRotation(targetPosition - transform.position).eulerAngles;

    private void LookTarget()
    {
        Vector3 rotation = GetRotation();
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), Time.deltaTime * rotationSpeed);
    }
}
