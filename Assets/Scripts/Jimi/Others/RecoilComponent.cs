using UnityEngine;

public class RecoilComponent : MonoBehaviour
{
    [SerializeField] Transform barrel;
    [SerializeField] float cooldown;
    [SerializeField] bool xAxis;
    [SerializeField] bool yAxis;
    [SerializeField] bool zAxis;

    [SerializeField] bool isDetached;

    private Vector3 axis;
    private Vector3 barrelStartPosition;
    private float recoil;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        axis = new Vector3(xAxis ? 1 : 0, yAxis ? 1 : 0, zAxis ? 1 : 0);
        barrelStartPosition = barrel.localPosition;
    }

    private void Update()
    {
        recoil = Mathf.Max(0, recoil - Time.deltaTime * (1 / cooldown));

        if (!isDetached)
            barrel.localPosition = barrelStartPosition + barrel.TransformDirection(axis) * -recoil;
        else
            barrel.localPosition = barrelStartPosition + barrel.InverseTransformDirection(barrel.up) * -recoil;
    }

    public void Add(float amount)
    {
        recoil = amount;
    }    
}
