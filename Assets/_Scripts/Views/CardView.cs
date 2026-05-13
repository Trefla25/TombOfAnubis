using DG.Tweening;
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private GameObject wrapper;
    [SerializeField] private LayerMask dropLayer;
    [SerializeField] private SpriteRenderer background;

    public Card Card { get; private set; }
    public bool IsPlayable { get; private set; } = true;

    private static readonly Color UnplayableTint = new(0.45f, 0.45f, 0.45f, 0.7f);

    private Vector3 dragStartPos;
    private Quaternion dragStartRot;
    private Color baseBackgroundColor;

    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        imageSR.sprite = card.Image;

        if (background != null && card.OwnerDog != null)
        {
            baseBackgroundColor = card.OwnerDog.Data.TintColor;
            background.color = baseBackgroundColor;
        }
        else if (background != null)
        {
            baseBackgroundColor = background.color;
        }

        RefreshPlayability();
    }

    public void RefreshPlayability()
    {
        if (Card == null) return;
        bool ownerAlive = Card.OwnerDog != null && Card.OwnerDog.IsAlive;
        bool canPay = ownerAlive && ManaSystem.Instance != null
                                && ManaSystem.Instance.HasEnoughMana(Card.Mana, Card.OwnerDog);
        IsPlayable = canPay;
        ApplyPlayabilityTint();
    }

    private void ApplyPlayabilityTint()
    {
        if (background == null) return;
        background.color = IsPlayable ? baseBackgroundColor : UnplayableTint;
    }

    void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        HandView.Instance.OnCardHover(this);
    }

    void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        HandView.Instance.OnCardUnhover(this);
    }

    void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (!IsPlayable) return;

        if (Card.ManualTargetEffects.Count > 0)
        {
            ManualTargetSystem.Instance.StartTargeting(imageSR.bounds.center);
        }
        else
        {
            Interactions.Instance.PlayerIsDragging = true;
            CardViewHoverSystem.Instance.Hide();
            dragStartPos = transform.position;
            dragStartRot = transform.rotation;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = MouseUtils.GetMousePositionInWorldSpace(-1);
            HandView.Instance.OnCardDragStart(this);
        }
    }

    void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (!IsPlayable) return;
        if (Card.ManualTargetEffects.Count > 0) return;
        transform.position = MouseUtils.GetMousePositionInWorldSpace(-1);
    }

    void OnMouseUp()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (!IsPlayable) return;

        if (Card.ManualTargetEffects.Count > 0)
        {
            var target = ManualTargetSystem.Instance.EndTargeting(MouseUtils.GetMousePositionInWorldSpace(-1));
            if (target != null && ManaSystem.Instance.HasEnoughMana(Card.Mana, Card.OwnerDog))
            {
                ActionSystem.Instance.Perform(new PlayCardGA(Card, target));
            }
        }
        else
        {
            HandView.Instance.OnCardDragEnd(this);
            if (ManaSystem.Instance.HasEnoughMana(Card.Mana, Card.OwnerDog)
                && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropLayer))
            {
                ActionSystem.Instance.Perform(new PlayCardGA(Card));
            }
            else
            {
                transform.position = dragStartPos;
                transform.rotation = dragStartRot;
            }
            Interactions.Instance.PlayerIsDragging = false;
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
