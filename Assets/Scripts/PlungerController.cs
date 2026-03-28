using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpringJoint))]
public class PlungerController : MonoBehaviour
{
    [Header("Spring Settings")]
    [SerializeField] private float springConstant = 50f;
    [SerializeField] private float damper = 10f;
    [SerializeField] private float maxPullDistance = 0.5f;
    [SerializeField] private float chargeRate = 0.3f;

    [Header("References")]
    [SerializeField] private Rigidbody anchorRigidbody;

    private Rigidbody rb;
    private SpringJoint springJoint;
    private Collider plungerCollider;
    private float currentPull = 0f;
    private bool isCharging = false;
    private bool hasLaunched = false;
    private Vector3 restPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionX |
                         RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezeRotation;

        restPosition = rb.position;

        springJoint = GetComponent<SpringJoint>();
        springJoint.connectedBody = anchorRigidbody;
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.connectedAnchor = Vector3.zero;
        springJoint.spring = springConstant;
        springJoint.damper = damper;
        springJoint.minDistance = 0f;
        springJoint.maxDistance = 0f;

        // Zero out bounciness so the ball just rests on the tip while charging
        plungerCollider = GetComponent<Collider>();
        plungerCollider.material = new PhysicsMaterial("PlungerMat")
        {
            bounciness = 0f,
            dynamicFriction = 0f,
            staticFriction = 0f,
            bounceCombine = PhysicsMaterialCombine.Minimum,
            frictionCombine = PhysicsMaterialCombine.Minimum
        };
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        if (isCharging)
        {
            currentPull += chargeRate * Time.fixedDeltaTime;
            currentPull = Mathf.Clamp(currentPull, 0f, maxPullDistance);
            rb.MovePosition(restPosition - transform.forward * currentPull);
        }
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.Space) && !hasLaunched)
            isCharging = true;

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
            Release();
    }

    private void Release()
    {
        isCharging = false;
        hasLaunched = true;

        // Store pull before clearing it
        float storedPull = currentPull;
        currentPull = 0f;

        // SpringJoint fires naturally + manual impulse using stored pull
        float forceMagnitude = springConstant * storedPull;
        rb.AddForce(transform.forward * forceMagnitude, ForceMode.Impulse);

        Invoke(nameof(ResetPlunger), 2f);
    }

    private void ResetPlunger()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.MovePosition(restPosition);
        hasLaunched = false;
    }
}