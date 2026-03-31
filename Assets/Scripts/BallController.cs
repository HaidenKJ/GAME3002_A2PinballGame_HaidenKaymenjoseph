using UnityEngine;

// Handles ball physics initialization and drain detection
public class BallController : MonoBehaviour
{
    [Header("Ball Settings")]
    // Layer mask for surfaces that destroy the ball (e.g. drain zone)
    [SerializeField] private LayerMask destroyBallLayer;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Notify GameManager if ball touches the drain layer
        if (((1 << collision.gameObject.layer) & destroyBallLayer) != 0)
            GameManager.Instance.BallDrained(collision.gameObject.name);
    }
}