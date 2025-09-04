using UnityEngine;

public class MovingSpinningSaw : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 moveDirection = Vector3.right;  // Axis of movement
    public float distance = 5f;                    // Travel distance
    public float moveSpeed = 2f;                   // Movement speed

    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.forward; // Axis of spin
    public float rotationSpeed = 360f;             // Degrees per second

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // Save initial position
    }

    void Update()
    {
        // Movement: ping-pong between startPos and (startPos + direction * distance)
        float offset = Mathf.PingPong(Time.time * moveSpeed, distance);
        transform.position = startPos + moveDirection.normalized * offset;

        // Rotation: spin around given axis
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().PlayerDied(); // Respawn logic
        }
    }
}
