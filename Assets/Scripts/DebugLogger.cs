using UnityEngine;
using UnityEngine.UI;

public class DebugLogger : MonoBehaviour
{
    [SerializeField] private Text displayText;

    private static DebugLogger instance;

    private void Awake()
    {
        instance = this;
    }

    public static void Log(string message)
    {
        if (instance == null) return;

        string timestamped = $"[{Time.time:F1}s] {message}";
        Debug.Log(timestamped);
        instance.displayText.text = timestamped;
    }
}