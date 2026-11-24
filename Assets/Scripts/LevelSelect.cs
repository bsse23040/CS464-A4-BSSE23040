using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A button for each level, made from what SaveData remembers: locked until it is unlocked, with its best time once
/// it has one. A button loads its level through the LevelLoader.
/// </summary>
public class LevelSelect : MonoBehaviour
{
    [SerializeField] Button[] buttons;           // one per level, in order
    [SerializeField] TMP_Text[] labels;
    [SerializeField] LevelLoader loader;

    public GameObject FirstOpenButton { get { return buttons[0].gameObject; } }   // level 1 is never locked

    void Awake()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int level = i + 1;                   // level 1 is build index 1
            buttons[i].onClick.AddListener(() => loader.Load(level));
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int level = i + 1;
            bool open = SaveData.IsUnlocked(level);
            buttons[i].interactable = open;      // a button that is not interactable is also skipped by the arrow keys
            if (!open) labels[i].text = "Level " + level + "    locked";
            else if (SaveData.HasBest(level)) labels[i].text = "Level " + level + "    best " + TimeFormat.Seconds(SaveData.BestTime(level));
            else labels[i].text = "Level " + level + "    not finished";
        }
    }
}
