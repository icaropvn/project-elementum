using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    public Button[] levelButtons;
    public GameObject[] lockIcons;
    public TMP_Text[] levelTexts;

    public Color enabledTextColor = Color.white;
    public Color disabledTextColor;

    void Start()
    {
        ColorUtility.TryParseHtmlString("#1D1D1D", out disabledTextColor);
        int highestUnlockedLevel = PlayerPrefs.GetInt("HighestUnlockedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool unlocked = i + 1 <= highestUnlockedLevel;

            levelButtons[i].interactable = unlocked;
            lockIcons[i].SetActive(!unlocked);

            levelTexts[i].color = unlocked
                ? enabledTextColor
                : disabledTextColor;
        }
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level0" + level);
    }

    public void Back()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
