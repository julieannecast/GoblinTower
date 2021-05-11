using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection
{
    Front,
    Right,
    Back,
    Left
}

public class CameraBehavior : MonoBehaviour
{
    public FacingDirection facingDirection;
    public FacingDirection lastFacingDirection;
    public float degree;
    public float additionalDegree;
    public float distance;

    public Vector3 cameraInitialPosition;
    public Vector3 cameraInitialRotation;

    public Vector3 playerCameraInitialPosition;
    public Vector3 playerCameraInitialRotation;

    public GameObject playerCamera;

    private bool closeView;
    private void Update()
    {
        if (!closeView)
        {
            Vector3 pos = cameraInitialPosition + Quaternion.AngleAxis(degree + additionalDegree, Vector3.up) * Vector3.forward * distance;
            transform.position = Vector3.LerpUnclamped(transform.position, pos, 8f * Time.deltaTime);
            transform.rotation =
            Quaternion.Lerp(transform.rotation,
            Quaternion.Euler(cameraInitialRotation.x, degree + cameraInitialRotation.y, cameraInitialRotation.z), 8f * Time.deltaTime);
        } 
        else
        {
            Vector3 pos = playerCamera.transform.position + playerCameraInitialPosition + Quaternion.AngleAxis(degree, Vector3.up) * Vector3.forward * 1;
            transform.position = Vector3.Lerp(transform.position, pos, 15f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.Euler(playerCameraInitialRotation.x, degree + playerCameraInitialRotation.y, playerCameraInitialRotation.z), 4f * Time.deltaTime);
        }
    }

    public void RotateRight()
    {
        lastFacingDirection = facingDirection;
        facingDirection = AddRight();
        degree -= 90f;
    }

    public void RotateLeft()
    {
        lastFacingDirection = facingDirection;
        facingDirection = AddLeft();
        degree += 90f;
    }

    private FacingDirection AddRight()
    {
        int dir = ((int)facingDirection) + 1;
        if (dir > 3)
            dir = 0;
        return (FacingDirection)dir;
    }
    private FacingDirection AddLeft()
    {
        int dir = ((int)facingDirection) - 1;
        if (dir < 0)
            dir = 3;
        return (FacingDirection)dir;
    }

    public void ChangeView()
    {
        if (!closeView)
            transform.parent = playerCamera.transform;
        else
            transform.parent.DetachChildren();

        closeView = !closeView;
    }
}
