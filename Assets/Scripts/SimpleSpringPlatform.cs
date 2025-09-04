using UnityEngine;

public class TimedSpringPlatform : MonoBehaviour
{
    [Header("Spring Motion")]
    public float upDistance = 0.5f;     // how far it goes up
    public float upSpeed = 5f;          // how fast it goes up
    public float downSpeed = 2f;        // how fast it returns down
    public float interval = 3f;         // seconds between activations

    [Header("Launch Settings")]
    public float launchForce = 15f;     // launch strength

    private Vector3 startPos;
    private bool goingUp = false;
    private bool isActive = false; // true only while moving up

    void Start()
    {
        startPos = transform.localPosition;
        InvokeRepeating(nameof(ActivateSpring), interval, interval);
    }

    void Update()
    {
        if (goingUp)
        {
            // move up
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPos + Vector3.up * upDistance, upSpeed * Time.deltaTime);

            // if reached top, start going down
            if (Vector3.Distance(transform.localPosition, startPos + Vector3.up * upDistance) < 0.01f)
            {
                goingUp = false;
                isActive = false; // spring is no longer "launching"
            }
        }
        else
        {
            // return back down smoothly
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPos, downSpeed * Time.deltaTime);
        }
    }

    void ActivateSpring()
    {
        goingUp = true;
        isActive = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isActive && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector3.up * launchForce, ForceMode.VelocityChange);
                isActive = false; // only launch once per activation
            }
        }
    }
}
