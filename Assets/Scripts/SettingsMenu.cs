using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [Header("Reset Progress")]
    public GameObject resetProgressDescription;
    public GameObject resetProgressButton;
    public GameObject resetProgressMessage;

    public float messageDuration = 3f;

    private Coroutine messageCoroutine;

    void Start()
    {
        resetProgressDescription.SetActive(false);
        resetProgressButton.SetActive(false);
        resetProgressMessage.SetActive(false);
    }

    public void ShowResetProgress()
    {
        resetProgressDescription.SetActive(true);
        resetProgressButton.SetActive(true);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("HighestUnlockedLevel");
        PlayerPrefs.DeleteKey("HasDoubleJump");
        PlayerPrefs.DeleteKey("HasWallJump");
        PlayerPrefs.DeleteKey("TotalDeaths");

        PlayerPrefs.Save();

        ShowResetMessage();
    }

    private void ShowResetMessage()
    {
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        messageCoroutine = StartCoroutine(ResetMessageCoroutine());
    }

    private IEnumerator ResetMessageCoroutine()
    {
        resetProgressMessage.SetActive(true);
        yield return new WaitForSeconds(messageDuration);
        resetProgressMessage.SetActive(false);
    }

    public void Back()
    {
        SceneManager.LoadScene("StartMenu");
    }
}