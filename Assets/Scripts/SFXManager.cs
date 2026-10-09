using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    private AudioSource audioSource;

    [Header("SFX")]
    public AudioClip deathSound;
    public AudioClip jumpSound;
    public AudioClip checkpointSound;
    public AudioClip upgradeSound;
    public AudioClip doorSound;

    private const float SFX_VOLUME = 0.2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, SFX_VOLUME);
        }
    }
}