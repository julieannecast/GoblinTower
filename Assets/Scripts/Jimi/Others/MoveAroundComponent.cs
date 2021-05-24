using UnityEngine;

public class MoveAroundComponent : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed;

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
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
    }
}
