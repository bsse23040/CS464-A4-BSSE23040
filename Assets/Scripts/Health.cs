using System;
using UnityEngine;

/// <summary>
/// The hearts. Health owns the hearts and the rule about how often they can be lost (the i-frames).
/// The GameManager owns the lives. Health does not know what happens when it is hurt: it raises an event,
/// and whoever cares (the health bar, the hit sound, the sparks, the red flash) is listening.
/// </summary>
public class Health : MonoBehaviour
{
    [SerializeField] int max = 3;
    [SerializeField] float iFrames = 1f;      // seconds of safety after a hit

    int current;
    float invulnerableUntil;
    Renderer body;

    public int Current => current;
    public int Max => max;

    // raised after a hit lands, with the hearts that are left and the most there can be
    public event Action<int, int> Damaged;
    // raised when the hearts are filled again, after a respawn
    public event Action<int, int> Restored;

    void Awake()
    {
        current = max;
        body = GetComponentInChildren<Renderer>();
    }

    // the player blinks while nothing can hurt them
    void Update()
    {
        bool hidden = Time.time < invulnerableUntil && Mathf.FloorToInt(Time.time * 10f) % 2 == 0;
        body.enabled = !hidden;
    }

    // Returns true when the hit landed. A hazard asks every physics step, so this is where the rule lives.
    public bool TakeDamage(int amount)
    {
        if (current <= 0 || Time.time < invulnerableUntil) return false;     // down already, or still protected
        if (GameManager.Instance.IsRespawning) return false;

        invulnerableUntil = Time.time + iFrames;                              // start the clock
        current -= amount;
        Damaged?.Invoke(current, max);
        if (current <= 0) GameManager.Instance.PlayerDied();                  // <= 0, not == 0: the crusher hits for 2
        return true;
    }

    // called by the GameManager after a respawn: full hearts and a moment of protection
    public void Restore(float protection)
    {
        current = max;
        invulnerableUntil = Time.time + protection;
        Restored?.Invoke(current, max);
    }
}
