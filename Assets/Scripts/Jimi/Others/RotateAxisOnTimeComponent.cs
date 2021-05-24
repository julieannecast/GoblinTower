using UnityEngine;

public class RotateAxisOnTimeComponent : MonoBehaviour
{
    [SerializeField] float angleOverTime; //L'angle d'un axe lorsque time == 1
    [SerializeField] bool xAxis;
    [SerializeField] bool yAxis;
    [SerializeField] bool zAxis;
    [SerializeField] bool reverse;
    [SerializeField] PingPongTimerComponent timer;

    private Vector3 axis;
    private Vector3 rotation;
    private int dir;
    private bool isRotating;

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
        rotation = Vector3.zero;
        axis = new Vector3(xAxis ? 1 : 0, yAxis ? 1 : 0, zAxis ? 1 : 0);
        dir = reverse ? -1 : 1;
        StartRotating();
    }

    private void Update()
    {
        if (isRotating)
            Rotate(timer.Time);
    }

    public void StartRotating()
    {
        isRotating = true;
    }

    public void StopRotating()
    {
        isRotating = false;
    }

    private void Rotate(float time)
    {
        rotation.x = angleOverTime * (time * dir) * axis.x;
        rotation.y = angleOverTime * (time * dir) * axis.y;
        rotation.z = angleOverTime * (time * dir) * axis.z;
        transform.eulerAngles = rotation;
    }
}
