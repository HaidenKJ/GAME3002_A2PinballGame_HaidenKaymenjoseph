using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Freeze for one frame so the ball spawns cleanly before physics kicks in
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Start()
    {
        // Release constraints after first frame so it settles naturally onto the surface
        rb.constraints = RigidbodyConstraints.None;
    }
}