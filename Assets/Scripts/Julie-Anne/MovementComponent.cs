using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] InputAction move;
    [SerializeField] CameraControl cam;
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
            var rotation = Quaternion.Euler(0, cam.coin * 90, 0);
            var direction = rotation * new Vector3(currentMove.x, 0, currentMove.y);
            //var angle = Vector3.SignedAngle(direction, Vector3.forward, Vector3.up);
            Debug.Log(direction);
            //Debug.Log(angle);
            transform.rotation = Quaternion.LookRotation(-direction);
            var destination = transform.position + direction;
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
        var hauteur = GetDifferenceHauteur(origine, destination);

        if (hauteur == 0 || hauteur == 1)
        {
            destination.y += hauteur;
            return true;
        }
        else if (hauteur < 0)
        {
            var direction = destination - origine;
            var hauteurLoin = GetDifferenceHauteur(origine, origine + direction * 2);
            if(hauteurLoin == 0)
            {
                destination = origine + direction * 2;
                return true;
            }
            else
            {
                destination.y += hauteur;
                return true;
            }
        }
        return false;    
    }

    //Trouver la différence de hauteur entre la position du joueur et la prochaine position
    private int GetDifferenceHauteur(Vector3 origine, Vector3 destination)
    {
        var charles = new Ray(new Vector3(destination.x, top, destination.z), Vector3.down);
        if(Physics.Raycast(charles, out var hit))
        {
            return Mathf.RoundToInt(hit.point.y - origine.y);
        }
        //Ne devrait pas arriver
        return int.MinValue;        
    }
}
