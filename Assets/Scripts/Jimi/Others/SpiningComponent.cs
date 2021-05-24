using UnityEngine;

public class SpiningComponent : MonoBehaviour
{
    [SerializeField] Vector3 rotationPerSecond;
    [SerializeField] Space space;

    private bool isSpining;

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
        StartSpining();
    }

    private void Update()
    {
        if (isSpining)
            Spin();
    }

    public void StartSpining()
    {
        isSpining = true;
    }

    public void StopSpining()
    {
        isSpining = false;
    }

    private void Spin()
    {
        transform.Rotate(rotationPerSecond * Time.deltaTime, space);
    }
}
