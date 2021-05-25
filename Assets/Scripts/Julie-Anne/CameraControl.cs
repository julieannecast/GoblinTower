using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [SerializeField] InputAction moveClockwise;
    [SerializeField] InputAction moveCounterclockwise;
    public int coin { get; private set; }
    private bool _isRotating;
    private float _angleRestant;
    private int _delta;
    public float futureHeight;
    private bool _coinUpdated;
    private const float Speed = 180f;

    // Start is called before the first frame update
    void Start()
    {
        SetCallbacks();
        moveClockwise.Enable();
        moveCounterclockwise.Enable();
        futureHeight = transform.position.y;
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
        if (!_isRotating)
        {
            _coinUpdated = false;
            _angleRestant = 90;
            _delta = delta;
            _isRotating = true;
        }
    }

    private void Update()
    {
        if (_isRotating)
        {
            _angleRestant -= Speed * Time.deltaTime;
            if (_angleRestant > 0)
            {
                if (_angleRestant < 45 && !_coinUpdated)
                {
                    coin = (4 + ((coin + _delta) % 4)) % 4;
                    _coinUpdated = true;
                }
                transform.RotateAround(new Vector3(), Vector3.up, _delta * Mathf.Min(_angleRestant, Speed * Time.deltaTime));
            }
            else
            {
                _isRotating = false;
            }
        }
        var futurePosition = new Vector3(transform.position.x, futureHeight, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, futurePosition, Time.deltaTime * 3);
    
    }
    private void OnDestroy()
    {
        moveClockwise.Disable();
        moveCounterclockwise.Disable();
    }

}
