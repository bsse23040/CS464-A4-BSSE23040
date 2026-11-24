using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The only script that loads a scene. It loads in the background behind a loading bar, so the screen never freezes.
/// The scene order is the Scene List: 0 MainMenu, 1 to 5 the levels, 6 Credits.
/// Every scene has its own LevelLoader. It is destroyed with the scene and made again by the next one.
/// </summary>
public class LevelLoader : MonoBehaviour
{
    [SerializeField] int menuIndex = 0;
    [SerializeField] int creditsIndex = 6;           // the last scene. Going on from here wraps back to the menu
    [SerializeField] GameObject loadingPanel;        // shown while a scene loads
    [SerializeField] Image bar;                      // an Image set to Filled, Horizontal
    [SerializeField] TMP_Text percentText;
    [SerializeField] float minimumShowTime = 0.35f;  // the scenes are small, so without this the bar would flash by

    bool loading;

    int Current { get { return gameObject.scene.buildIndex; } }

    public void Load(int index)
    {
        if (loading) return;
        StartCoroutine(LoadRoutine(index));
    }

    // one scene on. After the credits that would be a scene that does not exist, so go back to the menu
    public void LoadNext()
    {
        int next = Current + 1;
        if (Current == creditsIndex || next >= SceneManager.sceneCountInBuildSettings) next = menuIndex;
        Load(next);
    }

    public void Reload() { Load(Current); }

    public void LoadMenu() { Load(menuIndex); }

    IEnumerator LoadRoutine(int index)
    {
        loading = true;
        Time.timeScale = 1f;                         // timeScale survives a load, so it is put back here

        if (loadingPanel != null) loadingPanel.SetActive(true);
        SetBar(0f);

        AsyncOperation op = SceneManager.LoadSceneAsync(index);
        op.allowSceneActivation = false;             // hold the scene back at 90% so the bar can be seen to finish
        float shown = 0f;
        while (op.progress < 0.9f)
        {
            shown = Mathf.Clamp01(op.progress / 0.9f);   // progress stops at 0.9 until the scene is allowed to start
            SetBar(shown);
            yield return null;
        }

        SetBar(1f);
        yield return new WaitForSecondsRealtime(minimumShowTime);
        op.allowSceneActivation = true;
    }

    void SetBar(float value)
    {
        if (bar != null) bar.fillAmount = value;
        if (percentText != null) percentText.text = Mathf.RoundToInt(value * 100f) + "%";
    }
}
