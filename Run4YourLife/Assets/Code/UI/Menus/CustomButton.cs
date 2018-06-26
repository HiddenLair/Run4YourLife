using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Run4YourLife.GameManagement.AudioManagement;

public class CustomButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private AudioClip selectedButton;

    [SerializeField]
    private float buttonScaleMultiplierOnFocus = 1.25f;

    [SerializeField]
    private float textScaleMultiplierOnFocus = 1.25f;

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

    // Button scale smooth transition
    private float currentButtonScaleMultiplierOnFocus = 1.0f;
    private float currentTargetButtonScaleMultiplierOnFocus = 1.0f;

    // Text scale smooth transition
    private float currentTextScaleMultiplierOnFocus = 1.0f;
    private float currentTargetTextScaleMultiplierOnFocus = 1.0f;

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
        currentButtonScaleMultiplierOnFocus = Mathf.Lerp(currentButtonScaleMultiplierOnFocus, currentTargetButtonScaleMultiplierOnFocus, scaleTransitionSpeed * Time.unscaledDeltaTime);
        currentTextScaleMultiplierOnFocus = Mathf.Lerp(currentTextScaleMultiplierOnFocus, currentTargetTextScaleMultiplierOnFocus, scaleTransitionSpeed * Time.unscaledDeltaTime);

        currentColor = Color32.Lerp(currentColor, currentTargetColor, colorTransitionSpeed * Time.unscaledDeltaTime);

        rectTransform.sizeDelta = currentButtonScaleMultiplierOnFocus * initialButtonTransformSizeDelta;

        textMeshProUGUI.color = currentColor;
        textMeshProUGUI.transform.localScale = currentTextScaleMultiplierOnFocus * initialTextMeshScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetFocus(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (selectedButton != null)
        {
            AudioManager.Instance.PlaySFX(selectedButton);
        }

        SetFocus(false);
    }

    private void SetFocus(bool focus)
    {
        currentTargetButtonScaleMultiplierOnFocus = focus ? buttonScaleMultiplierOnFocus : 1.0f;
        currentTargetTextScaleMultiplierOnFocus = focus ? textScaleMultiplierOnFocus : 1.0f;

        currentTargetColor = focus ? colorOnFocus : colorNormal;
    }
}