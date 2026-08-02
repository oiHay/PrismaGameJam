using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonTextTint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private TMP_Text label;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightedColor = Color.white;
    [SerializeField] private Color pressedColor = Color.white;
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color disabledColor = Color.white;
    [SerializeField] private float fadeDuration = 0.1f;

    private Button _button;

    private void Awake() => _button = GetComponent<Button>();

    private void OnEnable() => Tint(normalColor);

    private void OnDisable()
    {
        if (label != null)
            label.CrossFadeColor(Color.white, 0f, true, true);
    }

    public void OnPointerEnter(PointerEventData eventData) => Tint(highlightedColor);
    public void OnPointerExit(PointerEventData eventData)  => Tint(normalColor);
    public void OnPointerDown(PointerEventData eventData)  => Tint(pressedColor);
    public void OnPointerUp(PointerEventData eventData)    => Tint(highlightedColor);
    public void OnSelect(BaseEventData eventData)        => Tint(selectedColor);
    public void OnDeselect(BaseEventData eventData)      => Tint(normalColor);

    private void Tint(Color color)
    {
        if (label == null || !_button.interactable) return;
        label.CrossFadeColor(color, fadeDuration, true, true);
    }
}