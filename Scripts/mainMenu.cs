using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public string newGameScene;
    public string leaderboardScene;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void Leaderboard()
    {
        SceneManager.LoadScene(leaderboardScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
