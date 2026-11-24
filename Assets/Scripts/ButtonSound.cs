using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The click sound of a menu button. It goes through the SfxPlayer, which is not destroyed when the click loads a scene.
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    [SerializeField] AudioClip clip;

    Button button;

    void Awake() { button = GetComponent<Button>(); }

    void OnEnable() { button.onClick.AddListener(OnClick); }
    void OnDisable() { button.onClick.RemoveListener(OnClick); }

    void OnClick()
    {
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(clip);
    }
}
