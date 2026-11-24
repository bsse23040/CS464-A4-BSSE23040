using UnityEngine;

/// <summary>
/// Plays the hit sound when the player is hurt. It is one of the listeners of Health.Damaged, so Health itself
/// never has to know that there is a sound.
/// </summary>
public class HitSound : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] AudioClip clip;

    void OnEnable() { health.Damaged += OnHit; }
    void OnDisable() { health.Damaged -= OnHit; }

    void OnHit(int current, int max)
    {
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(clip);
    }
}
