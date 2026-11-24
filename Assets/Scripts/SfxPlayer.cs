using UnityEngine;

/// <summary>
/// One player for every sound effect. Anything that makes a noise calls SfxPlayer.Instance.Play(clip).
/// It lives on its own object that is never destroyed with a coin or a level, so a sound is never cut off,
/// and each sound gets a slightly different pitch so the same one does not sound like a machine.
/// </summary>
public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer Instance { get; private set; }

    [SerializeField] AudioSource[] sources;                      // about four, all with Output set to the SFX group
    [SerializeField] float pitchRangeSemitones = 2f;             // a random pitch from -2 to +2 semitones

    int next;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }   // the same check as the music: one of these, ever
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // the pause menu stops the listener, and its own buttons still need to click
        foreach (var s in sources) s.ignoreListenerPause = true;
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource s = sources[next];
        next = (next + 1) % sources.Length;                      // take the next source, so overlapping sounds do not cut each other

        // a semitone is a factor of 2^(1/12), so n semitones is 2^(n/12)
        float semitones = Random.Range(-pitchRangeSemitones, pitchRangeSemitones);
        s.pitch = Mathf.Pow(2f, semitones / 12f);
        s.PlayOneShot(clip, volume);
    }
}
