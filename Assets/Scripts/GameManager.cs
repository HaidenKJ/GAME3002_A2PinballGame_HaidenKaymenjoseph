using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Ball Settings")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawn;
    [SerializeField] private int maxBalls = 3;

    private int ballsRemaining;
    private GameObject currentBall;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ballsRemaining = maxBalls;
        DebugLogger.Log($"Game started | Balls remaining: {ballsRemaining}");
    }

    private void Start()
    {
        RespawnBall();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            LoseBall("Manual reset");

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
        DebugLogger.Log("GAME OVER - No balls remaining!");
        if (currentBall != null) currentBall.SetActive(false);
    }

    private void RespawnBall()
    {
        if (currentBall != null) Destroy(currentBall);
        currentBall = Instantiate(ballPrefab, ballSpawn.position, Quaternion.identity);
        DebugLogger.Log($"Ball spawned | Balls remaining: {ballsRemaining}");
    }

    public int GetBallsRemaining() => ballsRemaining;
}