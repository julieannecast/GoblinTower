using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [SerializeField] InputAction moveClockwise;
    [SerializeField] InputAction moveCounterclockwise;
    public int coin { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        SetCallbacks();
        moveClockwise.Enable();
        moveCounterclockwise.Enable();
    }
    private void SetCallbacks()
    {
        moveClockwise.started += ctx =>
        {
            RotateAround(-1);
            
        };
        moveCounterclockwise.started += ctx =>
        {
            RotateAround(1);
        };
    }

    private void RotateAround(int delta)
    {
        coin = (4 + ((coin + delta) % 4)) % 4;
        transform.RotateAround(new Vector3(), Vector3.up, delta * 90);
    }

    private void OnDestroy()
    {
        moveClockwise.Disable();
        moveCounterclockwise.Disable();
    }

}
