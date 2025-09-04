using UnityEngine;

public class Collectable : MonoBehaviour
{
    private GameManager gameManager;

    [Header("Floating Motion")]
    public float floatAmplitude = 0.25f; // how high it moves up/down
    public float floatSpeed = 2f;        // speed of floating

    [Header("Rotation")]
    public float rotationSpeed = 90f;    // degrees per second

    private Vector3 startPos;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Floating (sin wave)
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);

        // Rotation (y-axis spin)
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.AddScore(1);
            gameObject.SetActive(false); // Disable instead of destroy
        }
    }
}
