using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DogSelectorCardView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image portrait;
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text classText;
    [SerializeField] private GameObject selectedFrame;

    public DogData Dog { get; private set; }
    private Action<DogSelectorCardView> onClick;

    public void Setup(DogData dog, Action<DogSelectorCardView> clickHandler)
    {
        Dog = dog;
        onClick = clickHandler;
        if (portrait != null) portrait.sprite = dog.Image;
        if (background != null) background.color = dog.TintColor;
        if (nameText != null) nameText.text = dog.DisplayName;
        if (hpText != null) hpText.text = "HP " + dog.Health;
        if (classText != null) classText.text = dog.Class.ToString();
        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        if (selectedFrame != null) selectedFrame.SetActive(selected);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      onClick?.Invoke(this);
    }
}