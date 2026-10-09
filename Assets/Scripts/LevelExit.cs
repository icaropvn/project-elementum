using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Level")]
    public int levelToUnlock;
    public float exitDelay = 0.3f;

    [Header("Door Requirements")]
    public bool requiresDoubleJump = false;
    public bool requiresWallJump = false;

    [Header("Door Visual")]
    public SpriteRenderer doorRenderer;
    public Sprite unlockedSprite;
    public Sprite lockedSprite;

    private PlayerMovement player;
    private bool triggered = false;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>();
        UpdateDoorVisual();
    }

    private void Update()
    {
        UpdateDoorVisual();
    }

    private bool IsLocked(PlayerMovement player)
    {
        if (requiresDoubleJump && !player.hasDoubleJump)
            return true;

        if (requiresWallJump && !player.hasWallJump)
            return true;

        return false;
    }

    private void UpdateDoorVisual()
    {
        if (player == null || doorRenderer == null)
            return;

        doorRenderer.sprite = IsLocked(player)
            ? lockedSprite
            : unlockedSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered)
            return;

        SFXManager.Instance.Play(
            SFXManager.Instance.doorSound
        );

        PlayerMovement collidedPlayer = collision.GetComponentInParent<PlayerMovement>();

        if (collidedPlayer == null)
            return;

        if (IsLocked(collidedPlayer))
            return;

        triggered = true;
        UnlockNextLevel();
        collidedPlayer.gameObject.SetActive(false);
        StartCoroutine(ReturnToLevelSelect());
    }

    private void UnlockNextLevel()
    {
        int highestUnlockedLevel = PlayerPrefs.GetInt("HighestUnlockedLevel", 1);

        if (levelToUnlock > highestUnlockedLevel)
        {
            PlayerPrefs.SetInt("HighestUnlockedLevel", levelToUnlock);
            PlayerPrefs.Save();
        }
    }

    private IEnumerator ReturnToLevelSelect()
    {
        yield return new WaitForSeconds(exitDelay);
        SceneManager.LoadScene("LevelSelectMenu");
    }
}