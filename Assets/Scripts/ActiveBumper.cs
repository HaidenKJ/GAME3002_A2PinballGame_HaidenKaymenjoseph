using UnityEngine;

// Active bumper, reflects the ball's velocity and awards points
public class ActiveBumper : MonoBehaviour
{
    [SerializeField] private int scoreValue = 100;      // Points awarded on hit
    [SerializeField] private float reflectForce = 10f;  // Speed of reflected ball

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        Rigidbody ballRb = collision.rigidbody;
        if (ballRb == null) return;

        // Reflect ball velocity off the bumper's collision normal (Vector3.Reflect)
        Vector3 reflectDir = Vector3.Reflect(collision.relativeVelocity.normalized, collision.contacts[0].normal);
        ballRb.linearVelocity = reflectDir * reflectForce;

        GameManager.Instance.AddScore(scoreValue, "Active Bumper");
    }
}