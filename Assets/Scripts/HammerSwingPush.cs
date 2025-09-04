using UnityEngine;

public class HammerSwingPush : MonoBehaviour
{
    public enum Axis { X, Y, Z }

    [Header("Swing")]
    [Tooltip("Local axis to swing around (relative to the hammer's starting orientation).")]
    public Axis swingAround = Axis.Z;
    [Tooltip("Maximum swing angle to each side, in DEGREES.")]
    public float swingAngle = 70f;
    [Tooltip("Swing speed (bigger = faster).")]
    public float swingSpeed = 2f;
    [Tooltip("Phase offset in DEGREES to desync multiple hammers.")]
    public float phaseOffsetDegrees = 0f;

    [Header("Pivot (no extra GameObjects needed)")]
    [Tooltip("Local-space offset from the hammer's origin to the hinge point.\n" +
             "If your hammer's pivot isn't at the hinge, tweak this until the rotation looks right.")]
    public Vector3 pivotLocalOffset = Vector3.zero;

    [Header("Hit / Push")]
    [Tooltip("Horizontal knockback strength applied to the player.")]
    public float pushForce = 18f;
    [Tooltip("Extra upward boost so the hit feels punchy.")]
    public float upwardBoost = 4f;

    // --- internals ---
    private Quaternion _startLocalRot;
    private Vector3 _startWorldPos;
    private Quaternion _startWorldRot;
    private Vector3 _pivotWorld;
    private Vector3 _axisWorld;
    private float _prevAngleDeg;

    void Awake()
    {
        // Cache starting transform
        _startLocalRot  = transform.localRotation;
        _startWorldPos  = transform.position;
        _startWorldRot  = transform.rotation;

        // Compute a FIXED world-space pivot from the starting pose
        _pivotWorld = _startWorldPos + _startWorldRot * pivotLocalOffset;

        // Build the world-space swing axis from the starting orientation
        Vector3 localAxis =
            swingAround == Axis.X ? Vector3.right :
            swingAround == Axis.Y ? Vector3.up    :
                                    Vector3.forward;

        _axisWorld = (_startWorldRot * localAxis).normalized;

        _prevAngleDeg = 0f;
    }

    void Update()
    {
        // Angle(t) = A * sin(ω t + φ)
        float phaseRad = phaseOffsetDegrees * Mathf.Deg2Rad;
        float angleNowDeg = swingAngle * Mathf.Sin(Time.time * swingSpeed + phaseRad);

        // Rotate only by the DELTA since last frame around a fixed pivot+axis
        float deltaDeg = angleNowDeg - _prevAngleDeg;
        transform.RotateAround(_pivotWorld, _axisWorld, deltaDeg);

        _prevAngleDeg = angleNowDeg;
    }

    // --- Push logic works with either collider type ---
    void OnCollisionEnter(Collision collision)
    {
        TryPushPlayerFromCollision(collision);
    }
    void OnTriggerEnter(Collider other)
    {
        TryPushPlayerFromTrigger(other, other.ClosestPoint(transform.position));
    }

    private void TryPushPlayerFromCollision(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        Rigidbody rb = collision.rigidbody != null
            ? collision.rigidbody
            : collision.collider.GetComponent<Rigidbody>();

        if (rb == null) return;

        Vector3 contactPoint = collision.GetContact(0).point;
        ApplyPush(rb, contactPoint);
    }

    private void TryPushPlayerFromTrigger(Collider other, Vector3 contactPoint)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody rb = other.attachedRigidbody != null
            ? other.attachedRigidbody
            : other.GetComponent<Rigidbody>();

        if (rb == null) return;

        ApplyPush(rb, contactPoint);
    }

    private void ApplyPush(Rigidbody playerRb, Vector3 contactPoint)
    {
        // Tangential direction of the hammer at the contact point:
        // v_dir ~ ω × r  (we only need the direction)
        Vector3 r = (contactPoint - _pivotWorld);
        Vector3 tangentialDir = Vector3.Cross(_axisWorld, r).normalized;

        // Scale a bit by instantaneous swing speed (|cos| term)
        float phaseRad = phaseOffsetDegrees * Mathf.Deg2Rad;
        float speedScale = Mathf.Abs(Mathf.Cos(Time.time * swingSpeed + phaseRad)); // 0..1

        Vector3 force = tangentialDir * (pushForce * Mathf.Lerp(0.6f, 1f, speedScale))
                        + Vector3.up * upwardBoost;

        playerRb.AddForce(force, ForceMode.Impulse);
    }

    // Visual aid in Scene view
    void OnDrawGizmosSelected()
    {
        // Recompute preview using current (editor) transform so it's easy to tune
        Quaternion startWorldRot = Application.isPlaying ? _startWorldRot : transform.rotation;
        Vector3 startWorldPos    = Application.isPlaying ? _startWorldPos : transform.position;

        Vector3 pivotWorld = startWorldPos + startWorldRot * pivotLocalOffset;

        Vector3 localAxis =
            swingAround == Axis.X ? Vector3.right :
            swingAround == Axis.Y ? Vector3.up    :
                                    Vector3.forward;
        Vector3 axisWorld = (startWorldRot * localAxis).normalized;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pivotWorld, 0.15f);
        Gizmos.DrawLine(pivotWorld, pivotWorld + axisWorld * 0.8f);
    }
}
