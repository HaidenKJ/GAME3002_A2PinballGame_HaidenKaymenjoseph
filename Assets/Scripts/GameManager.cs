using UnityEngine;
using TMPro;

// Manages game state including score, ball count, and win/lose conditions
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Ball Settings")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawn;
    [SerializeField] private int maxBalls = 3;

    [Header("Score Settings")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int ballsRemaining;
    private int currentScore;
    private GameObject currentBall;

    private void Awake()
    {
        // Singleton pattern, only one GameManager allowed
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ballsRemaining = maxBalls;
        currentScore = 0;
        UpdateScoreUI();
    }

    private void Start()
    {
        RespawnBall();
    }

    private void Update()
    {
        // R resets the ball manually
        if (Input.GetKeyDown(KeyCode.R))
            LoseBall("Manual reset");

        // 1 removes a ball, 2 adds a ball (debug only)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ballsRemaining = Mathf.Max(0, ballsRemaining - 1);
            DebugLogger.Log($"Balls decreased | Balls remaining: {ballsRemaining}");
            if (ballsRemaining <= 0) GameOver();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ballsRemaining++;
            DebugLogger.Log($"Balls increased | Balls remaining: {ballsRemaining}");
        }
    }
    
    // Called by bumpers, bash toy, and any other scoring objects
    public void AddScore(int points, string source)
    {
        currentScore += points;
        UpdateScoreUI();
        DebugLogger.Log($"{source} gave {points} points | Total: {currentScore}");
    }

    // Called by BallController when ball touches the drain layer
    public void BallDrained(string drainedBy)
    {
        LoseBall(drainedBy);
    }

    private void LoseBall(string reason)
    {
        ballsRemaining--;
        DebugLogger.Log($"Ball lost ({reason}) | Balls remaining: {ballsRemaining}");

        if (ballsRemaining <= 0)
        {
            GameOver();
            return;
        }

        RespawnBall();
    }

    private void GameOver()
    {
        DebugLogger.Log($"GAME OVER | Final Score: {currentScore}");
        if (currentBall != null) currentBall.SetActive(false);
    }

    private void RespawnBall()
    {
        if (currentBall != null) Destroy(currentBall);
        currentBall = Instantiate(ballPrefab, ballSpawn.position, Quaternion.identity);
        DebugLogger.Log($"Ball spawned | Balls remaining: {ballsRemaining}");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"SCORE: {currentScore}";
    }

    public int GetBallsRemaining() => ballsRemaining;
    public int GetScore() => currentScore;
}