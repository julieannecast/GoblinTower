using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] InputAction move;
    private Vector2 currentMove;
    private float moveCooldown;
    const float top = 50;


    void Awake()
    {
        SetCallbacks();
        move.Enable();
    }
    private void SetCallbacks()
    {
        move.performed += ctx =>
        {
            currentMove = ctx.ReadValue<Vector2>();
            Debug.Log(currentMove);

        };
        move.canceled += ctx =>
        {
            currentMove = new Vector3();
        };
    }
    private void Update()
    {
        if (moveCooldown > 0)
        {
            moveCooldown -= Time.fixedDeltaTime;
        }
        else if(currentMove.magnitude > 0)
        {
            var destination = new Vector3(transform.position.x + currentMove.x, 
                                          transform.position.y, 
                                          transform.position.z+currentMove.y);
            if(DeplacementValide(transform.position, ref destination))
            {
                transform.position = destination;
                moveCooldown = 0.2f;
            }
            
        }
    }

    private void OnDestroy()
    {
        move.Disable();
    }
    private bool DeplacementValide(Vector3 origine, ref Vector3 destination)
    {
        //le nom de variable est une joke, pls pas trop me taper dessus
        var charles = new Ray(new Vector3(destination.x, top, destination.z), Vector3.down);
        if(Physics.Raycast(charles, out var hit))
        {
            var hauteur = Mathf.RoundToInt(hit.point.y - origine.y);
            if(hauteur <= 1)
            {
                destination.y = hit.point.y;
                return true;
            }
        }
        return false;
    }
}
