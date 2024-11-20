using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Dan.Main;
using System.Threading;

public class LeaderboardManager : MonoBehaviour
{
    public string mainMenuScence;

    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;
    private string publicLeaderBoardKey = "67a71c6f4a847443f7b1e6c053fe97aaf7783e4926c8e7cf374c5485e1ed54fb";

    void Start()
    {
        LeaderboardCreator.ResetPlayer();
        GetLeaderBoard();
    }

    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderBoardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; ++i)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderBoardKey, username, score, ((msg) =>
        {
            GetLeaderBoard();
            LeaderboardCreator.ResetPlayer();
        }));
    }

    public void ReturnToMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScence);
    }
}
