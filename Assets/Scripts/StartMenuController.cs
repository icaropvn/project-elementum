using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    void Start()
    {
        
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("LevelSelectMenu");
    }

    public void OnSettingsClick()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void OnExitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
