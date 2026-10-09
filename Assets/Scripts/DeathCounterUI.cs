using TMPro;
using UnityEngine;

public class DeathCounterUI : MonoBehaviour
{
    public TMP_Text deathText;

    void Start()
    {
        UpdateText();
    }

    public void AddDeath()
    {
        int deaths = PlayerPrefs.GetInt("TotalDeaths", 0);

        deaths++;

        PlayerPrefs.SetInt("TotalDeaths", deaths);
        PlayerPrefs.Save();

        UpdateText();
    }

    private void UpdateText()
    {
        int deaths = PlayerPrefs.GetInt("TotalDeaths", 0);

        deathText.text = "Mortes: " + deaths;
    }
}