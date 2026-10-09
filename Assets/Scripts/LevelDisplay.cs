using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDisplay : MonoBehaviour
{
    public TMP_Text levelText;

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.StartsWith("Level"))
        {
            string levelNumber = sceneName.Replace("Level", "");
            levelText.text = levelNumber;
        }
    }
}