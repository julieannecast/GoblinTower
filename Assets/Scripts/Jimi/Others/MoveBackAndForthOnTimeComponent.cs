using UnityEngine;

public class MoveBackAndForthOnTimeComponent : MonoBehaviour
{
    [SerializeField] Transform startTarget;
    [SerializeField] Transform endTarget;
    [SerializeField] bool ignoreYAxis;
    [SerializeField] PingPongTimerComponent timer;

    private Vector3 nextPosition;
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
        StartMoving();
    }

    private void Update()
    {
        if (isMoving)
            Move(timer.Time);
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    private void Move(float time)
    {
        nextPosition = Vector3.Lerp(startTarget.position, endTarget.position, time);
        if (ignoreYAxis)
            nextPosition.y = transform.position.y;
        transform.position = nextPosition;
    }
}
