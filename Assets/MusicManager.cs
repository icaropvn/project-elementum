using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource audioSource;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip level3Music;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();

            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource.volume = 0.025f;
        audioSource.loop = true;

        ChangeMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeMusicForScene(scene.name);
    }

    private void ChangeMusicForScene(string sceneName)
    {
        AudioClip newMusic = null;

        switch (sceneName)
        {
            case "StartMenu":
            case "LevelSelectMenu":
                newMusic = menuMusic;
                break;

            case "Level01":
                newMusic = level1Music;
                break;

            case "Level02":
                newMusic = level2Music;
                break;

            case "Level03":
                newMusic = level3Music;
                break;
        }

        if (newMusic != null)
        {
            PlayBackgroundMusic(newMusic);
        }
    }

    private void PlayBackgroundMusic(AudioClip audioClip)
    {
        if (audioSource.clip == audioClip)
            return;

        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}