using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    public void PauseInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LevelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelectMenu");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }
}