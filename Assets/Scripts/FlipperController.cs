using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(Rigidbody))]
public class FlipperController : MonoBehaviour
{
    [Header("Spring Settings")]
    [SerializeField] private float m_fSpringConst = 2000.0f;       // Spring force of the hinge
    [SerializeField] private float m_fOriginalPos = -30.0f;        // Resting angle of the flipper
    [SerializeField] private float m_fPressedPos = 30.0f;          // Flipped angle of the flipper
    [SerializeField] private float m_fFlipperSpringDamp = 50.0f;   // Damping to prevent oscillation

    [Header("Input")]
    [SerializeField] private KeyCode m_FlipperInput;               // Key assigned to this flipper

    private HingeJoint m_hingeJoint = null;
    private JointSpring m_jointSpring;

    void Start()
    {
        m_hingeJoint = GetComponent<HingeJoint>();
        m_hingeJoint.useSpring = true;

        // Configure the spring via script using Hooke's Law principles
        m_jointSpring = new JointSpring();
        m_jointSpring.spring = m_fSpringConst;
        m_jointSpring.damper = m_fFlipperSpringDamp;
        m_jointSpring.targetPosition = m_fOriginalPos;
        m_hingeJoint.spring = m_jointSpring;
    }

    // Rotate flipper to pressed position when input is held
    private void OnFlipperPressedInternal()
    {
        m_jointSpring.targetPosition = m_fPressedPos;
        m_hingeJoint.spring = m_jointSpring;
    }

    // Return flipper to resting position when input is released
    private void OnFlipperReleasedInternal()
    {
        m_jointSpring.targetPosition = m_fOriginalPos;
        m_hingeJoint.spring = m_jointSpring;
    }

    void Update()
    {
        if (Input.GetKeyDown(m_FlipperInput))
            OnFlipperPressedInternal();
        else if (Input.GetKeyUp(m_FlipperInput))
            OnFlipperReleasedInternal();
    }
}