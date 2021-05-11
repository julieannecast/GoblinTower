using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float degree;

    public Vector3 direction = Vector3.zero;
    public Vector3 lastDirection = Vector3.zero;
    public Vector3 desiredPosition;

    public float movementSpeed = 20f;
    public float acceleration = 3f;
    public float jumpForce = 300f;
    public float gravityMultiplier;

    public bool isMoving;
    public bool isOnGround;

    public float playerHeight = 1f;
    public Rigidbody rg;
    public CameraBehavior camera;
    public Transform skin;

    private void Start()
    {
        rg = GetComponent<Rigidbody>();
        desiredPosition = transform.position;
    }
    private void Update()
    {
        degree = camera.degree;

        if (direction.magnitude > 0)
        {
            lastDirection = direction;
            desiredPosition = (transform.position + transform.TransformDirection(direction)).Round();
        }

        Debug.DrawRay(transform.position, Vector3.down * (playerHeight + 0.3f), Color.yellow);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degree, 0), 12 * Time.deltaTime);
        
        SetSkinDegree();
        
        Move();
        isOnGround = CheckIfOnGround();
    }

    void FixedUpdate()
    {
        rg.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }
    public void Move()
    {
        Vector3 desiredVelocity = (desiredPosition - transform.position) * movementSpeed;
        Vector3 velocity = rg.velocity;
        velocity.x = Mathf.Lerp(velocity.x, desiredVelocity.x, acceleration * Time.deltaTime);
        velocity.y = rg.velocity.y;
        velocity.z = Mathf.Lerp(velocity.z, desiredVelocity.z, acceleration * Time.deltaTime);
        rg.velocity = velocity;
    }
    public void Jump()
    {
        if (isOnGround)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
        }
    }
    public void SetSkinDegree()
    {
        Vector3 locRot = transform.TransformDirection(lastDirection);
        skin.rotation = Quaternion.Slerp(skin.rotation, Quaternion.LookRotation(new Vector3(-locRot.z, 0, locRot.x)), 15f * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "")
        {

        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "")
        {

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "")
        {

        }
    }

    private bool CheckIfOnGround()
    {
        var ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, (playerHeight + 0.3f)))
        {
            Debug.Log(hit.collider.gameObject.ToString());
            return true;
        }

        return false;
    }
}
