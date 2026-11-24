using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// The pause menu. Esc opens it, and it stops three things: Time.timeScale (physics, guards, timers), the audio
/// listener (the sound), and the player's own input. Resume puts all three back, and it runs before every load:
/// Time.timeScale, AudioListener.pause and the static IsPaused all survive a scene load, so a restart without it
/// would come back frozen or silent.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [SerializeField] GameObject panel;
    [SerializeField] GameObject firstButton;     // selected as the menu opens, so the arrow keys work at once
    [SerializeField] LevelLoader loader;

    // with Enter Play Mode options turned on, statics are not reset by themselves
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatic() { IsPaused = false; }

    // Update still runs at a time scale of 0, which is why Esc can also close the menu
    void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame) return;
        if (IsPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (GameManager.Instance != null && GameManager.Instance.Ended) return;   // a win or game over screen is already up
        IsPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        panel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        panel.SetActive(false);
    }

    public void Restart()
    {
        Resume();                                // before the load, never after
        loader.Reload();
    }

    public void QuitToMenu()
    {
        Resume();
        loader.LoadMenu();
    }
}
