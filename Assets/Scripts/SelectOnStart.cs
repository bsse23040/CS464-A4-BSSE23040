using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Selects one button when the scene starts, so the arrow keys and Enter work without touching the mouse first.
/// The EventSystem's own First Selected would do the same for a menu that is there from the start.
/// </summary>
public class SelectOnStart : MonoBehaviour
{
    [SerializeField] GameObject first;

    void Start()
    {
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(first);
    }
}
