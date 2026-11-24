using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The whole interface of a level in one prefab: the HUD, the pause menu, the win and game over screens and the
/// loading bar. A fix in the prefab reaches all five levels. This script only holds the pieces and shows the end
/// screens. The GameManager decides when.
/// </summary>
public class LevelUI : MonoBehaviour
{
    [Header("HUD")]
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public GameObject hintPanel;                 // a solid panel behind the hint, shown only while there is a hint
    public TMP_Text hintText;
    public GameObject boostPanel;
    public TMP_Text boostText;

    [Header("Win screen")]
    public GameObject winPanel;
    public TMP_Text winTitle;
    public TMP_Text winTimeText;
    public TMP_Text winNextLabel;
    public GameObject winFirstButton;

    [Header("Game over screen")]
    public GameObject losePanel;
    public GameObject loseFirstButton;

    public void SetHint(string message)
    {
        hintPanel.SetActive(message.Length > 0);
        hintText.text = message;
    }

    public void SetBoost(string message)
    {
        boostPanel.SetActive(message.Length > 0);
        boostText.text = message;
    }

    public void ShowWin(string title, string nextLabel, float time, bool newBest, float best)
    {
        winTitle.text = title;
        winNextLabel.text = nextLabel;
        winTimeText.text = newBest
            ? "Time " + TimeFormat.Seconds(time) + "\nNew best time!"
            : "Time " + TimeFormat.Seconds(time) + "\nBest " + TimeFormat.Seconds(best);
        winPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(winFirstButton);
    }

    public void ShowLose()
    {
        losePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(loseFirstButton);
    }
}
