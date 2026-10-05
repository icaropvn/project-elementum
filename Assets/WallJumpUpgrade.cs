using UnityEngine;

public class WallJumpUpgrade : MonoBehaviour
{
    [Header("Floating")]
    public float floatHeight = 0.2f;
    public float floatSpeed = 2f;

    [Header("UI")]
    public GameObject upgradeIcon;

    private Vector3 startPosition;

    void Start()
    {
        if (PlayerPrefs.GetInt("HasWallJump", 0) == 1)
        {
            Destroy(gameObject);
            return;
        }

        startPosition = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.UnlockWallJump();

            SFXManager.Instance.Play(
                SFXManager.Instance.upgradeSound
            );

            if (upgradeIcon != null)
            {
                upgradeIcon.SetActive(true);
            }

            Destroy(gameObject);
        }
    }
}
