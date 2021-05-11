using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputsComponent : MonoBehaviour
{
    [SerializeField] private PlayerBehavior player;
    [SerializeField] private CameraBehavior camera;

    [SerializeField] InputAction moveForward;
    [SerializeField] InputAction moveBackward;
    [SerializeField] InputAction moveRight;
    [SerializeField] InputAction moveLeft;
    [SerializeField] InputAction jump;
    [SerializeField] InputAction moveCameraRight;
    [SerializeField] InputAction moveCameraLeft;
    [SerializeField] InputAction changeCameraView;

    private void Awake()
    {
        EnableInputs();
        SetCallbacks();
    }

    private void SetCallbacks()
    {
        moveForward.performed += ctx => player.direction += Vector3.forward;
        moveBackward.performed += ctx => player.direction += Vector3.back;
        moveRight.performed += ctx => player.direction += Vector3.right;
        moveLeft.performed += ctx => player.direction += Vector3.left;

        moveForward.canceled += ctx => player.direction -= Vector3.forward;
        moveBackward.canceled += ctx => player.direction -= Vector3.back;
        moveRight.canceled += ctx => player.direction -= Vector3.right;
        moveLeft.canceled += ctx => player.direction -= Vector3.left;

        jump.performed += ctx => player.Jump();

        moveCameraRight.performed += ctx => camera.RotateRight();
        moveCameraLeft.performed += ctx => camera.RotateLeft();
        changeCameraView.performed += ctx => camera.ChangeView();
    }

    private void EnableInputs()
    {
        moveForward.Enable();
        moveBackward.Enable();
        moveRight.Enable();
        moveLeft.Enable();
        jump.Enable();
        moveCameraRight.Enable();
        moveCameraLeft.Enable();
        changeCameraView.Enable();
    }

    private void OnDestroy()
    {
        moveForward.Disable();
        moveBackward.Disable();
        moveRight.Disable();
        moveLeft.Disable();
        jump.Disable();
        moveCameraRight.Disable();
        moveCameraLeft.Disable();
        changeCameraView.Disable();
    }
}
