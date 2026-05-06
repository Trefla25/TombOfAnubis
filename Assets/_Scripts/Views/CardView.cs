using System;
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

    public Card Card { get; private set; }
    private Vector3 dragStartPos;
    private Quaternion dragStartRot;
    
    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        imageSR.sprite = card.Image;
    }

    void OnMouseEnter()
    {
        if(!Interactions.Instance.PlayerCanHover()) return;
        HandView.Instance.OnCardHover(this);
    }

    void OnMouseExit()
    {
        if(!Interactions.Instance.PlayerCanHover()) return;
        HandView.Instance.OnCardUnhover(this);
    }

    void OnMouseDown()
    {
        if(!Interactions.Instance.PlayerCanInteract()) return;
        if(Card.ManualTargetEffect != null)
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
        if(!Interactions.Instance.PlayerCanInteract()) return;
        if(Card.ManualTargetEffect != null) return;
        transform.position = MouseUtils.GetMousePositionInWorldSpace(-1);
    }

    void OnMouseUp()
    {
        if(!Interactions.Instance.PlayerCanInteract()) return;
        if(Card.ManualTargetEffect != null)
        {
            var target = ManualTargetSystem.Instance.EndTargeting(MouseUtils.GetMousePositionInWorldSpace(-1));
            if(target != null 
                    && ManaSystem.Instance.HasEnoughMana(Card.Mana))
            {
                PlayCardGA playCardGA = new(Card, target);
                ActionSystem.Instance.Perform(playCardGA);
            }
        }
        else
        {
            HandView.Instance.OnCardDragEnd(this);
            if(ManaSystem.Instance.HasEnoughMana(Card.Mana) 
                    && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropLayer))
            {
                PlayCardGA playCardGA = new(Card);
                ActionSystem.Instance.Perform(playCardGA);
            }
            else
            {
                transform.position = dragStartPos;
                transform.rotation = dragStartRot;
            }

            Interactions.Instance.PlayerIsDragging = false;            
        }
    }
}
