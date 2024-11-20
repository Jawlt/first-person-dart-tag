using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public PlayerController playerController;
    public EnemyAI enemyAI;
    public UIManager uiManager;
    public LeaderboardManager leaderboardManager;

    [Header("Game Settings")]
    public float gameDuration = 180f; // 3 minutes in seconds

    private float remainingTime;
    private float playerItTime = 0f;
    private float enemyItTime = 0f;
    private bool gameIsOver = false;

    private bool playerIsIt;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        remainingTime = gameDuration;
        AssignRandomIt();
        uiManager.UpdateTimerDisplay(remainingTime);
    }

    void Update()
    {
        if (!gameIsOver)
        {
            UpdateTimer();
            TrackItTime();
        }
    }

    void AssignRandomIt()
    {
        playerIsIt = Random.value > 0.5f;
        UpdateItStatus();
    }

    void TrackItTime()
    {
        if (playerIsIt)
        {
            playerItTime += Time.deltaTime;
        }
        else
        {
            enemyItTime += Time.deltaTime;
        }
    }

    public void SwitchItStatus()
    {
        if (gameIsOver) return;

        playerIsIt = !playerIsIt;
        UpdateItStatus();
    }

    void UpdateItStatus()
    {
        playerController.SetIt(playerIsIt);
        enemyAI.SetIt(!playerIsIt);
        uiManager.UpdateItStatus(playerIsIt ? "Player" : "Enemy");
    }

    void UpdateTimer()
    {
        remainingTime -= Time.deltaTime;
        uiManager.UpdateTimerDisplay(remainingTime);

        if (remainingTime <= 0 && !gameIsOver)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameIsOver = true;

        string winner = playerItTime < enemyItTime ? "Player" : "Enemy";
        int score = playerItTime < enemyItTime ? (int)playerItTime : (int)enemyItTime;
        winner += Random.Range(1000, 9999);

        uiManager.ShowEndGameMessage($"{winner} Wins! Score: {score}");

        leaderboardManager.SetLeaderboardEntry(winner, score);

        Invoke(nameof(LoadLeaderboardScene), 3f);
    }

    void LoadLeaderboardScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Leaderboard");
    }

    public void ResetGame()
    {
        playerItTime = 0f;
        enemyItTime = 0f;
        remainingTime = gameDuration;
        gameIsOver = false;

        AssignRandomIt();
        uiManager.UpdateTimerDisplay(remainingTime);
    }
}
