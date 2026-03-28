using UnityEngine;

public class BallDebugger : MonoBehaviour
{
    [SerializeField] private float speedThreshold = 5f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.linearVelocity.magnitude > speedThreshold)
        {
            DebugLogger.Log($"Ball exceeded speed threshold! Speed: {rb.linearVelocity.magnitude:F2}");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DebugLogger.Log($"Ball collided with: {collision.gameObject.name} | " +
                        $"Material: {collision.collider.material.name} | " +
                        $"Impulse: {collision.impulse.magnitude:F2}");
    }

    private void OnTriggerEnter(Collider other)
    {
        DebugLogger.Log($"Ball triggered: {other.gameObject.name}");
    }
}