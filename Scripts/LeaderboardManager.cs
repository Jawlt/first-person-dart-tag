using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    public string mainMenuScence;
    public TextMeshProUGUI playerTimeText;
    public TextMeshProUGUI enemyTimeText;

    void Start()
    {
        // Retrieve and display data
        float playerItTime = PlayerPrefs.GetFloat("PlayerItTime", 0f);
        float enemyItTime = PlayerPrefs.GetFloat("EnemyItTime", 0f);

        playerTimeText.text = $"Player 'It' Time: {playerItTime:F2} seconds";
        enemyTimeText.text = $"Enemy 'It' Time: {enemyItTime:F2} seconds";
    }

    public void ReturnToMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScence);
    }
}
