using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public GameObject doubleJumpIcon;
    public GameObject wallJumpIcon;

    void Start()
    {
        doubleJumpIcon.SetActive(PlayerPrefs.GetInt("HasDoubleJump", 0) == 1);
        wallJumpIcon.SetActive(PlayerPrefs.GetInt("HasWallJump", 0) == 1);
    }
}