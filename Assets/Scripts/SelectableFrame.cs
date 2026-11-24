using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Shows which button is selected. The arrow keys move the selection and the mouse pointer takes it too, so there is
/// only one highlight to look for. A light outline appears around the selected button and the button grows a little.
/// </summary>
public class SelectableFrame : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    [SerializeField] Outline outline;             // switched off until the button is selected
    [SerializeField] float selectedScale = 1.05f;

    public void OnSelect(BaseEventData eventData) { Show(true); }
    public void OnDeselect(BaseEventData eventData) { Show(false); }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // the mouse takes the selection, so a mouse and a keyboard never leave two buttons lit
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(gameObject);
    }

    void OnDisable() { Show(false); }

    void Show(bool on)
    {
        if (outline != null) outline.enabled = on;
        transform.localScale = Vector3.one * (on ? selectedScale : 1f);
    }
}
