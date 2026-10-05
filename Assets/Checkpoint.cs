using UnityEngine;
using UnityEngine.Tilemaps;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint;
    public Tilemap flagTilemap;
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated)
            return;

        PlayerMovement player = collision.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.SetCheckpoint(respawnPoint.position);
            activated = true;

            SFXManager.Instance.Play(
                SFXManager.Instance.checkpointSound
            );

            flagTilemap.color = new Color(1f, 0.8f, 0f);
        }
    }
}
