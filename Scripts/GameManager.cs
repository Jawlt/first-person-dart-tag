using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerController playerController;
    public EnemyAI enemyAI;
    public UIManager uiManager;

    public float gameDuration = 180f; // 3 minutes in seconds
    private float remainingTime;
    private float playerItTime = 0f;
    private float enemyItTime = 0f;
    private bool gameIsOver = false;

    private bool playerIsIt;

    void Start()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

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

        if (playerIsIt)
        {
            playerController.SetIt(true);
            enemyAI.SetIt(false);
            uiManager.UpdateItStatus("Player");
        }
        else
        {
            playerController.SetIt(false);
            enemyAI.SetIt(true);
            uiManager.UpdateItStatus("Enemy");
        }
    }

    void TrackItTime()
    {
        // Accumulate time for whoever is currently "it"
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
        // Switch "it" status between player and enemy
        playerIsIt = !playerIsIt;
        playerController.SetIt(playerIsIt);
        enemyAI.SetIt(!playerIsIt);
        uiManager.UpdateItStatus(playerIsIt ? "Player" : "Enemy");
    }

    void UpdateTimer()
    {
        remainingTime -= Time.deltaTime;
        uiManager.UpdateTimerDisplay(remainingTime);

        if (remainingTime <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameIsOver = true;

        // Determine who was "it" the longest
        string winner = playerItTime < enemyItTime ? "Player" : "Enemy";
        uiManager.ShowEndGameMessage($"{winner} wins!");

        PlayerPrefs.SetFloat("PlayerItTime", playerItTime);
        PlayerPrefs.SetFloat("EnemyItTime", enemyItTime);
        PlayerPrefs.Save();

        Invoke("LoadLeaderboardScene", 3f);
    }

    void LoadLeaderboardScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Leaderboard");
    }
}
