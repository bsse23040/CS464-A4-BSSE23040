using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A red flash over the screen when the player is hurt, fading out in about a third of a second.
/// The third listener of Health.Damaged, after the health bar and the hit sound.
/// </summary>
public class DamageFlash : MonoBehaviour
{
    [SerializeField] Image flash;                // a full-screen Image that does not block clicks
    [SerializeField] float strength = 0.22f;     // the opacity right after a hit
    [SerializeField] float seconds = 0.3f;

    Health health;
    float left;

    void OnEnable()
    {
        health = FindAnyObjectByType<Health>();
        if (health != null) health.Damaged += OnHit;
    }

    void OnDisable()
    {
        if (health != null) health.Damaged -= OnHit;
    }

    void OnHit(int current, int max) { left = seconds; }

    void Update()
    {
        left = Mathf.Max(0f, left - Time.deltaTime);
        Color c = flash.color;
        c.a = strength * (left / seconds);
        flash.color = c;
    }
}
