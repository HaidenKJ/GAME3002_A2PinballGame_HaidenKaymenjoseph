using UnityEngine;
using UnityEngine.UI;

// Displays a single debug message on screen via a Legacy Text component
public class DebugLogger : MonoBehaviour
{
    [SerializeField] private Text displayText;

    private static DebugLogger instance;

    private void Awake()
    {
        instance = this;
    }

    // Call from anywhere to update the onscreen debug message
    public static void Log(string message)
    {
        if (instance == null) return;

        string timestamped = $"[{Time.time:F1}s] {message}";
        Debug.Log(timestamped);
        instance.displayText.text = timestamped;
    }
}