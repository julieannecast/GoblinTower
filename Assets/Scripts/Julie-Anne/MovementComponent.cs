using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class MovementComponent : MonoBehaviour
{
    [SerializeField] InputAction _move;
    [SerializeField] CameraControl _cam;
    private CourbeBezier _courbe;
    private float _tempsMouvement;
    private Vector2 _currentMove;
    private bool _isMoving;
    private Vector3 _destination;
    const float top = 50;
    private const float Speed = 2.0f;
    private Animator _anim;

    void Awake()
    {
        SetCallbacks();
        _anim = GetComponent<Animator>();
        _move.Enable();
    }

    private void SetCallbacks()
    {
        _move.started += ctx =>
        {
            Debug.Log("pressed");
            if(!_isMoving)
                _currentMove = ctx.ReadValue<Vector2>();
        };
    }

    private void Update()
    {
        if (_isMoving) 
        {
            _tempsMouvement += Time.deltaTime * Speed;
            if (_tempsMouvement >= 1.0f)
            {
                transform.position = _destination;
                _isMoving = false;
                _anim.SetBool("isMoving", false);
            }
            else
            {
                transform.position = _courbe.Evaluer(_tempsMouvement);
            }
        }        
        else if(_currentMove.magnitude > 0)
        {
            var rotation = Quaternion.Euler(0, _cam.coin * 90, 0);
            var direction = rotation * new Vector3(_currentMove.x, 0, _currentMove.y);
            transform.rotation = Quaternion.LookRotation(-direction);
            var destination = transform.position + direction;
            if(DeplacementValide(transform.position, ref destination))
            {
                _destination = destination;
                _isMoving = true;
                _anim.SetBool("isMoving", true);
                var distance = _destination - transform.position;
                var milieu = transform.position + distance / 2;
                var sommetY = Math.Max(transform.position.y, _destination.y) + 0.5f;
                var sommet = new Vector3(milieu.x,sommetY,milieu.z);
                _courbe = new CourbeBezier(transform.position, sommet, _destination);
                _tempsMouvement = 0;
            }
            _currentMove = new Vector2();
        }
    }

    private void OnDestroy()
    {
        _move.Disable();
    }

    private bool DeplacementValide(Vector3 origine, ref Vector3 destination)
    {
        var hauteurs = GetTabDifferenceHauteur(origine, destination);

        foreach (var hauteur in hauteurs)
        {
            if (hauteur == 0 || hauteur == 1)
            {
                destination.y += hauteur;
                return true;
            }
            else if (hauteur < 0)
            {
                var direction = destination - origine;
                var hauteursLoin = GetTabDifferenceHauteur(origine, origine + direction * 2);
                foreach (var hauteurLoin in hauteursLoin)
                {
                    if (hauteurLoin == 0)
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
            }
        }
        return false;
    }

    //Trouver la différence de hauteur entre la position du joueur et la prochaine position
    private int[] GetTabDifferenceHauteur(Vector3 origine, Vector3 destination)
    {
        var hits = Physics.RaycastAll(new Vector3(destination.x, top, destination.z), Vector3.down, 100f, LayerMask.GetMask("Ground"));
        int[] differences = new int[hits.Length];
        for(int i = 0; i < differences.Length; i++) 
        { 
            differences[i] = Mathf.RoundToInt(hits[i].point.y - origine.y);
        }
        Array.Sort(differences, (a, b) => b.CompareTo(a));
        return differences;    
    }
}
