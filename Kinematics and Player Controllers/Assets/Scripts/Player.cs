using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    CharacterController controller = null;
    Animator animator = null;

    public float speed = 80.0f;
    public float pushPower = 2.0f;

    public Vector3 velocity;
    public bool grounded;
    public float jumpSpeed = 5;
    public float jumpHeight = 5;

	// Use this for initialization
	void Start ()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        velocity = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        velocity += Physics.gravity * Time.deltaTime;

        RaycastHit hit;
        grounded = Physics.Raycast(transform.position + 0.5f * Vector3.up, Vector3.down, out hit, 0.55f);

        jumpSpeed = Mathf.Sqrt(-2.0f * Physics.gravity.y * jumpHeight);
        if (grounded)
        {
            // zero our vertical velocity
            if (velocity.y < 0)
                velocity.y = hit.point.y - transform.position.y;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // jump
                velocity = Vector3.up * jumpSpeed;
            }
        }

        controller.Move(velocity * Time.deltaTime);
        transform.Rotate(transform.up, horizontal * speed * Time.deltaTime);
        animator.SetFloat("Speed", vertical * speed * Time.deltaTime);
  	}

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3f)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }
}
