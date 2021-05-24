using UnityEngine;

/*
 * Note: La position du target est calculée une seule fois. -> Ne change pas. -> Est fixe.
 */
public class MoveTowardComponent : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float speed;
    public Vector3 TargetPosition { get; private set; }

    private bool isMoving;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        SetTargetPosition();
        StartMoving();
    }

    private void Update()
    {
        if (isMoving)
            Move();
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    private void Move()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, speed * Time.deltaTime);
    }

    public float DistanceFromTarget() =>
        Vector3.Distance(transform.position, TargetPosition);

    private void SetTargetPosition()
    {
        TargetPosition = target.position + offset;
    }
}
