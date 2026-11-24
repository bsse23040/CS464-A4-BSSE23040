using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// The main menu has two panels: the first with Play, Level select and Quit, the second with one button per level.
/// Each panel selects its first button as it opens, so the whole menu works from the keyboard.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject levelPanel;
    [SerializeField] LevelSelect levelSelect;
    [SerializeField] LevelLoader loader;

    void Start() { ShowMain(); }

    // Esc goes back from the level list to the first panel
    void Update()
    {
        if (!levelPanel.activeSelf) return;
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) ShowMain();
    }

    public void ShowMain()
    {
        levelPanel.SetActive(false);
        mainPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void ShowLevels()
    {
        mainPanel.SetActive(false);
        levelPanel.SetActive(true);
        levelSelect.Refresh();                   // built from SaveData each time the list opens
        EventSystem.current.SetSelectedGameObject(levelSelect.FirstOpenButton);
    }

    // Play goes to the newest level that is open, which is Level 1 on the first run
    public void Play() { loader.Load(SaveData.Unlocked); }

    public void Quit() { Application.Quit(); }
}
