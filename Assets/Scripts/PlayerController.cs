using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;          // Movement speed
    public float jumpForce = 5f;       // Jump strength
    private Rigidbody rb;              // Reference to the Rigidbody

    private float movementX;           // Input along X
    private float movementY;           // Input along Y
    private bool isGrounded;           // Is the player on the ground?

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movement input
        movementX = Input.GetAxis("Horizontal");
        movementY = Input.GetAxis("Vertical");

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Prevent double jump
        }
    }

    void FixedUpdate()
    {
        // Move the player with force
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If we touch the ground, allow jumping again
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
