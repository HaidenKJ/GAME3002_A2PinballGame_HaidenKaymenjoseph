using UnityEngine;

// Passive bumper, slows the ball down on contact and awards points
public class PassiveBumper : MonoBehaviour
{
    [SerializeField] private int scoreValue = 50;       // Points awarded on hit
    [SerializeField] private float dampenAmount = 0.5f; // Multiplier applied to ball velocity (0-1)

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        Rigidbody ballRb = collision.rigidbody;
        if (ballRb == null) return;

        // Reduce ball velocity without redirecting it
        ballRb.linearVelocity *= dampenAmount;

        GameManager.Instance.AddScore(scoreValue, "Passive Bumper");
    }
}