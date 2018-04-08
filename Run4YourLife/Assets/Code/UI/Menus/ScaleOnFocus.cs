using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleOnFocus : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private float scaleMultiplier = 1.25f;

    private RectTransform rectTransform;
    private Vector2 initialSizeDelta;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialSizeDelta = rectTransform.sizeDelta;
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetFocus(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SetFocus(false);
    }

    private void SetFocus(bool focus)
    {
        rectTransform.sizeDelta = (focus ? scaleMultiplier : 1.0f) * initialSizeDelta;
    }
}