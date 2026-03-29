using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(Rigidbody))]
public class FlipperController : MonoBehaviour
{
    [SerializeField] private float m_fSpringConst = 2000.0f;
    [SerializeField] private float m_fOriginalPos = -30.0f;
    [SerializeField] private float m_fPressedPos = 30.0f;
    [SerializeField] private float m_fFlipperSpringDamp = 50.0f;
    [SerializeField] private KeyCode m_FlipperInput;

    private HingeJoint m_hingeJoint = null;
    private JointSpring m_jointSpring;

    void Start()
    {
        m_hingeJoint = GetComponent<HingeJoint>();
        m_hingeJoint.useSpring = true;

        m_jointSpring = new JointSpring();
        m_jointSpring.spring = m_fSpringConst;
        m_jointSpring.damper = m_fFlipperSpringDamp;
        m_jointSpring.targetPosition = m_fOriginalPos;
        m_hingeJoint.spring = m_jointSpring;

        DebugLogger.Log($"Flipper {gameObject.name} initialized | Key: {m_FlipperInput}");
    }

    private void OnFlipperPressedInternal()
    {
        m_jointSpring.targetPosition = m_fPressedPos;
        m_hingeJoint.spring = m_jointSpring;
        DebugLogger.Log($"Flipper {gameObject.name} pressed");
    }

    private void OnFlipperReleasedInternal()
    {
        m_jointSpring.targetPosition = m_fOriginalPos;
        m_hingeJoint.spring = m_jointSpring;
        DebugLogger.Log($"Flipper {gameObject.name} released");
    }

    void Update()
    {
        if (Input.GetKeyDown(m_FlipperInput))
            OnFlipperPressedInternal();
        else if (Input.GetKeyUp(m_FlipperInput))
            OnFlipperReleasedInternal();
    }
}