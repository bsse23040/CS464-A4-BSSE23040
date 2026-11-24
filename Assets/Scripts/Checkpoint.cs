using UnityEngine;

/// <summary>
/// A trigger the player walks through. After it, a lost life brings them back here and not to the start.
/// </summary>
public class Checkpoint : MonoBehaviour
{
    [SerializeField] AudioClip clip;

    bool reached;

    void OnTriggerEnter(Collider other)
    {
        if (reached || !other.CompareTag("Player")) return;
        reached = true;                                  // only the first time, so it does not say it again
        GameManager.Instance.SetCheckpoint(transform);
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(clip);
    }
}
