using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlungerController : MonoBehaviour
{
    [Header("Spring Settings")]
    [SerializeField] private float springConstant = 50f;    // k in Hooke's Law (F = kx)
    [SerializeField] private float damper = 10f;            // Dampens oscillation after launch
    [SerializeField] private float maxPullDistance = 0.5f;  // Maximum charge distance (x in F = kx)
    [SerializeField] private float chargeRate = 0.3f;       // How fast the plunger pulls back per second
    [SerializeField] private float adjustRate = 10f;        // How fast arrow keys adjust values

    [Header("References")]
    [SerializeField] private Rigidbody anchorRigidbody;     // Kinematic anchor the spring joint connects to

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

        // Constrain plunger to only slide along its forward axis
        rb.constraints = RigidbodyConstraints.FreezePositionX |
                         RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezeRotation;

        restPosition = rb.position;

        // Zero out bounciness so the ball rests on the tip without bouncing during charge
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

    private void Start()
    {
        // SpringJoint added in Start so it initializes one frame after the ball spawns
        // preventing physics solver interference on spawn
        springJoint = gameObject.AddComponent<SpringJoint>();
        springJoint.connectedBody = anchorRigidbody;
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.connectedAnchor = Vector3.zero;
        springJoint.spring = springConstant;
        springJoint.damper = damper;
        springJoint.minDistance = 0f;
        springJoint.maxDistance = 0f;
    }

    private void Update()
    {
        HandleInput();
        HandleTuning();
    }

    private void HandleTuning()
    {
        // Up/Down arrows adjust spring constant
        if (Input.GetKey(KeyCode.UpArrow))
        {
            springConstant += adjustRate * Time.deltaTime;
            springJoint.spring = springConstant;
            DebugLogger.Log($"Spring Constant: {springConstant:F1}");
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            springConstant = Mathf.Max(0f, springConstant - adjustRate * Time.deltaTime);
            springJoint.spring = springConstant;
            DebugLogger.Log($"Spring Constant: {springConstant:F1}");
        }

        // Left/Right arrows adjust damper
        if (Input.GetKey(KeyCode.RightArrow))
        {
            damper += adjustRate * Time.deltaTime;
            springJoint.damper = damper;
            DebugLogger.Log($"Damper: {damper:F1}");
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            damper = Mathf.Max(0f, damper - adjustRate * Time.deltaTime);
            springJoint.damper = damper;
            DebugLogger.Log($"Damper: {damper:F1}");
        }
    }

    private void FixedUpdate()
    {
        if (isCharging)
        {
            // Accumulate pull distance up to max, then hold position via physics
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

        // Store pull before clearing, F = kx where x is storedPull
        float storedPull = currentPull;
        currentPull = 0f;

        // Apply impulse force, SpringJoint releases naturally alongside this
        float forceMagnitude = springConstant * storedPull;
        rb.AddForce(transform.forward * forceMagnitude, ForceMode.Impulse);

        Invoke(nameof(ResetPlunger), 2f);
    }

    private void ResetPlunger()
    {
        // Kill all momentum and return plunger to rest position
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.MovePosition(restPosition);
        hasLaunched = false;
    }
}