using UnityEngine;

/// <summary>
/// A burst of sparks on the player when a hit lands. The Particle System is a child of the player with Looping off
/// and Play On Awake off, so it fires once per hit and then stops.
/// </summary>
public class HitSparks : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] ParticleSystem sparks;

    void OnEnable() { health.Damaged += OnHit; }
    void OnDisable() { health.Damaged -= OnHit; }

    void OnHit(int current, int max) { sparks.Play(); }
}
