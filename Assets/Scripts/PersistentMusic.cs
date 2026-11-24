using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// The music. It sits on a root object in every scene, so each scene can be played on its own. The first one that
/// wakes up stays for the rest of the game and every later copy destroys itself, so the music never doubles
/// and never restarts when a level loads.
/// </summary>
public class PersistentMusic : MonoBehaviour
{
    static PersistentMusic instance;

    [SerializeField] AudioSource source;     // looping, with its Output set to the Music group
    [SerializeField] AudioMixer mixer;

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }   // a copy from the next scene: the first one stays
        instance = this;
        DontDestroyOnLoad(gameObject);
        source.Play();                                           // started here, not by Play On Awake, so a copy never makes a sound
    }

    // a mixer parameter set in Awake is sometimes lost, so the saved volumes go on in Start
    void Start()
    {
        if (instance != this) return;
        MixerVolumes.ApplySaved(mixer);
    }
}
