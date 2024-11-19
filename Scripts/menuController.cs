using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{
    public string mainMenuScence;
    public GameObject pauseMenu;
    public bool isPaused;

    private PlayerController playerController; // Reference to the PlayerController

    void Start()
    {
        // Find the PlayerController in the scene
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Set the PlayerController's isPaused to true
        if (playerController != null)
        {
            playerController.isPaused = true;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set the PlayerController's isPaused to false
        if (playerController != null)
        {
            playerController.isPaused = false;
        }
    }

    public void ReturnToMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScence);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
