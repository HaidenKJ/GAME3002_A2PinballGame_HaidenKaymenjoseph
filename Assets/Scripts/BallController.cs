using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private LayerMask destroyBallLayer;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Start()
    {
        rb.constraints = RigidbodyConstraints.None;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & destroyBallLayer) != 0)
            GameManager.Instance.BallDrained(collision.gameObject.name);
    }
}