using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    private CharacterController personnage;
    [SerializeField] InputAction move;


    // Start is called before the first frame update
    void Awake()
    {
        personnage = GetComponent<CharacterController>();
        SetCallbacks();
        move.Enable();
    }
    private void SetCallbacks()
    {
        move.performed += ctx =>
        {
            var vecteur = ctx.ReadValue<Vector2>();
            personnage.Move(new Vector3(vecteur.x, 0, vecteur.y));

        };
    }

    private void OnDestroy()
    {
        move.Disable();
    }
}
