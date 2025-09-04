using UnityEngine;

public class SpinningBlade : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 75f;         // speed of spinning
    public Vector3 rotationAxis = Vector3.up;  // default spin on Y axis

    void Update()
    {
        // Rotate continuously
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Call your GameManager death logic
            FindObjectOfType<GameManager>().PlayerDied();
        }
    }
}
