using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PositioningSlotView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image portrait;
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text emptyLabel;
    [SerializeField] private GameObject highlight;

    public DogRow Row { get; private set; }
    public DogColumn Column { get; private set; }
    public DogData AssignedDog { get; private set; }
    public bool IsEmpty => AssignedDog == null;

    private Action<PositioningSlotView> onClick;

    public void Setup(DogRow row, DogColumn column, Action<PositioningSlotView> clickHandler)
    {
        Row = row;
        Column = column;
        onClick = clickHandler;
        ClearAssignment();
        SetHighlight(false);
    }

    public void Assign(DogData dog)
    {
        AssignedDog = dog;
        if (portrait != null)
        {
            portrait.sprite = dog.Image;
            portrait.enabled = true;
        }
        if (background != null) background.color = dog.TintColor;
        if (emptyLabel != null) emptyLabel.enabled = false;
    }

    public void ClearAssignment()
    {
        AssignedDog = null;
        if (portrait != null) portrait.enabled = false;
        if (background != null) background.color = new Color(1, 1, 1, 0.3f);
        if (emptyLabel != null) emptyLabel.enabled = true;
    }

    public void SetHighlight(bool on)
    {
        if (highlight != null) highlight.SetActive(on);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(this);
    }
}