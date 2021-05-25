using UnityEngine;
using UnityEngine.Events;

/*
 * Note: 'CallbackOnJump' offre la possibilité d'appeler une fonction lors d'un saut.
 *        Exemple: Ralentir la vitesse d'un Timer Ping Pong.
 *        
 *        'Jump' est public. Cela permet de forcer un saut depuis l'extérieur.
 */

[RequireComponent(typeof(Rigidbody))]
public class AutoJumpOnFallComponent : MonoBehaviour
{
    [SerializeField] float jumpForce;
    [SerializeField] float distanceToGroundBeforeJump;
    [SerializeField] UnityEvent CallbackOnJump;
    [SerializeField] LayerMask IgnoredLayer;

    private bool jumped;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        JumpCheck();
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce);
        jumped = true;
    }

    private bool IsGrounded()
    {
        var ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, distanceToGroundBeforeJump, ~IgnoredLayer))
        {
            jumped = false;
            return true;
        }

        return false;
    }

    private void JumpCheck()
    {
        if (!IsGrounded() && !jumped)
        {
            CallbackOnJump.Invoke();
            Jump();
        }
    }
}
