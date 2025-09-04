using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Spike Motion")]
    public float moveDistance = 1f;     // how far the spikes rise up
    public float speed = 2f;            // speed of up/down motion
    public float waitTime = 1f;         // pause at top and bottom

    [Header("Damage Settings")]
    public int damage = 1;              // how much damage it deals
    public string playerTag = "Player"; // tag of the player

    private Vector3 startPos;
    private bool movingUp = true;
    private float timer;

    void Start()
    {
        startPos = transform.localPosition;
        timer = waitTime;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        // Determine target position
        Vector3 target = startPos + (movingUp ? Vector3.up * moveDistance : Vector3.zero);

        // Move toward target
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);

        // Check if reached
        if (Vector3.Distance(transform.localPosition, target) < 0.01f)
        {
            movingUp = !movingUp;
            timer = waitTime; // pause before switching
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            // if you have a health script on the player, call it here
            Debug.Log("Player hit by spikes! Damage: " + damage);

            // Example:
            // collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
