using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PositioningTokenView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image portrait;
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject highlight;
    [SerializeField] private GameObject placedOverlay;

    public DogData Dog { get; private set; }
    public bool IsPlaced { get; private set; }

    private Action<PositioningTokenView> onClick;

    public void Setup(DogData dog, Action<PositioningTokenView> clickHandler)
    {
        Dog = dog;
        onClick = clickHandler;
        if (portrait != null) portrait.sprite = dog.Image;
        if (background != null) background.color = dog.TintColor;
        if (nameText != null) nameText.text = dog.DisplayName;
        SetHighlight(false);
        SetPlaced(false);
    }

    public void SetHighlight(bool on)
    {
        if (highlight != null) highlight.SetActive(on);
    }

    public void SetPlaced(bool placed)
    {
        IsPlaced = placed;
        if (placedOverlay != null) placedOverlay.SetActive(placed);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsPlaced) return;
        onClick?.Invoke(this);
    }
}