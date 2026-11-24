using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The health bar: two Images filled from the left. The front one is the health, the paler one behind it is the
/// "lag bar". A hit drops the front bar at once, and the lag bar waits a moment and then follows, so the player can see
/// how much was just lost. HealthBar listens to Health's events and never asks it every frame.
/// </summary>
public class HealthBar : MonoBehaviour
{
    [SerializeField] Image fill;                 // Image Type Filled, Horizontal
    [SerializeField] Image lag;                  // the same, behind it and paler
    [SerializeField] float lagDelay = 0.4f;      // seconds before the lag bar starts to move
    [SerializeField] float lagSpeed = 0.8f;      // then it falls this much of the whole bar per second

    Health health;
    float waitLeft;

    void OnEnable()
    {
        health = FindAnyObjectByType<Health>();
        if (health == null) return;
        health.Damaged += OnChanged;
        health.Restored += OnChanged;
    }

    void OnDisable()
    {
        if (health == null) return;
        health.Damaged -= OnChanged;
        health.Restored -= OnChanged;
    }

    // Health sets its hearts in Awake, so the first reading waits for Start
    void Start()
    {
        if (health != null) Set(health.Current, health.Max);
    }

    void OnChanged(int current, int max) { Set(current, max); }

    public void Set(int current, int max)
    {
        float target = max > 0 ? Mathf.Clamp01((float)current / max) : 0f;
        if (target >= fill.fillAmount) lag.fillAmount = target;     // getting health back: nothing to drag behind
        else waitLeft = lagDelay;                                   // losing health: the lag bar waits first
        fill.fillAmount = target;
    }

    void Update()
    {
        if (lag.fillAmount <= fill.fillAmount) return;
        if (waitLeft > 0f) { waitLeft -= Time.deltaTime; return; }
        lag.fillAmount = Mathf.MoveTowards(lag.fillAmount, fill.fillAmount, lagSpeed * Time.deltaTime);
    }
}
