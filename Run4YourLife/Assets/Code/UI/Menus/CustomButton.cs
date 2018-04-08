using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private float scaleMultiplierOnFocus = 1.25f;

    [SerializeField]
    private Color32 colorNormal = new Color32(151, 255, 1, 255);

    [SerializeField]
    private Color32 colorOnFocus = new Color32(1, 118, 255, 255);
    // private Color32 colorOnFocus = new Color32(255, 1, 1, 255);

    [SerializeField]
    private float scaleTransitionSpeed = 7.5f;

    [SerializeField]
    private float colorTransitionSpeed = 15.0f;

    private RectTransform rectTransform;
    private TextMeshProUGUI textMeshProUGUI;

    private Vector2 initialButtonTransformSizeDelta;
    private Vector3 initialTextMeshScale;

    // Scale smooth transition
    private float currentScaleMultiplierOnFocus = 1.0f;
    private float currentTargetScaleMultiplierOnFocus = 1.0f;

    // Color smooth transition
    private Color32 currentColor;
    private Color32 currentTargetColor;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();

        initialButtonTransformSizeDelta = rectTransform.sizeDelta;
        initialTextMeshScale = textMeshProUGUI.transform.localScale;

        currentColor = colorNormal;
        currentTargetColor = colorNormal;
    }

    void Update()
    {
        currentScaleMultiplierOnFocus = Mathf.Lerp(currentScaleMultiplierOnFocus, currentTargetScaleMultiplierOnFocus, scaleTransitionSpeed * Time.deltaTime);
        currentColor = Color32.Lerp(currentColor, currentTargetColor, colorTransitionSpeed * Time.deltaTime);

        rectTransform.sizeDelta = currentScaleMultiplierOnFocus * initialButtonTransformSizeDelta;

        textMeshProUGUI.color = currentColor;
        textMeshProUGUI.transform.localScale = currentScaleMultiplierOnFocus * initialTextMeshScale;
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
        currentTargetScaleMultiplierOnFocus = focus ? scaleMultiplierOnFocus : 1.0f;
        currentTargetColor = focus ? colorOnFocus : colorNormal;
    }
}