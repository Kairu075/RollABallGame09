using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;   // The player (ball) to follow
    private Vector3 offset;     // Distance between camera and player

    void Start()
    {
        // Calculate and store the offset value
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        // Update camera position after player moves
        transform.position = player.transform.position + offset;
    }
}
