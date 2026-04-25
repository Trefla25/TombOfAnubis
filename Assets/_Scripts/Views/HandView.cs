using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class HandView : Singleton<HandView>
{
    [SerializeField] private SplineContainer splineContainer;

    [Header("Fan Layout")]
    [Range(0.01f, 0.3f)]
    [SerializeField] private float cardSpacing = 0.1f;
    [SerializeField] private float cardScale = 1f;
    [SerializeField] private float repositionDuration = 0.15f;
    [SerializeField] private bool allowCardRepositioning = true;

    [Header("Hover")]
    [SerializeField] private bool zoomOnHover = true;
    [SerializeField] private float hoverScale = 1.25f;
    [SerializeField] private bool resetRotationOnHover = true;
    [SerializeField] private float hoverLiftAmount = 0.5f;
    [Tooltip("Z position of the hovered card. More negative = closer to camera (in front of others).")]
    [SerializeField] private float hoverZDepth = -0.5f;
    [SerializeField] private float hoverSpreadOffset = 0.4f;
    [SerializeField] private float hoverAnimDuration = 0.12f;

    [Header("Drag")]
    [SerializeField] private float dragReorderDuration = 0.1f;

    public bool IsCardHovered => hoveredCard != null;

    private readonly List<CardView> cards = new();
    private CardView hoveredCard;
    private CardView draggedCard;

    private void Update()
    {
        if (draggedCard != null && allowCardRepositioning)
            UpdateDraggedCardOrder();
    }

    public IEnumerator AddCard(CardView cardView)
    {
        cards.Add(cardView);
        yield return UpdateCardPosition(repositionDuration);
    }

    public CardView RemoveCard(Card card)
    {
        var cardView = GetCardView(card);
        if (cardView == null) return null;
        if (hoveredCard == cardView) hoveredCard = null;
        if (draggedCard == cardView) draggedCard = null;
        cards.Remove(cardView);
        StartCoroutine(UpdateCardPosition(repositionDuration));
        return cardView;
    }

    public void OnCardHover(CardView card)
    {
        hoveredCard = card;
        int idx = cards.IndexOf(card);
        if (idx < 0) return;

        StartCoroutine(UpdateCardPosition(hoverAnimDuration));

        var splinePos = GetSplinePositionForIndex(idx);
        var targetPos = splinePos + Vector3.up * hoverLiftAmount;
        targetPos.z = hoverZDepth;

        card.transform.DOKill();
        card.transform.DOMove(targetPos, hoverAnimDuration);

        if (resetRotationOnHover)
            card.transform.DORotate(Vector3.zero, hoverAnimDuration);

        if (zoomOnHover)
            card.transform.DOScale(cardScale * hoverScale, hoverAnimDuration);
    }

    public void OnCardUnhover(CardView card)
    {
        if (hoveredCard == card) hoveredCard = null;
        StartCoroutine(UpdateCardPosition(hoverAnimDuration));
    }

    public void OnCardDragStart(CardView card)
    {
        if (hoveredCard == card) hoveredCard = null;
        draggedCard = card;
        card.transform.DOKill();
        card.transform.DOScale(cardScale, 0.1f);
        StartCoroutine(UpdateCardPosition(hoverAnimDuration));
    }

    public void OnCardDragEnd(CardView card)
    {
        draggedCard = null;
        StartCoroutine(UpdateCardPosition(repositionDuration));
    }

    private void UpdateDraggedCardOrder()
    {
        float mouseX = MouseUtils.GetMousePositionInWorldSpace(-1).x;
        int newIdx = cards.Count(c => c != draggedCard && mouseX > c.transform.position.x);
        int oldIdx = cards.IndexOf(draggedCard);
        if (newIdx == oldIdx) return;

        cards.RemoveAt(oldIdx);
        newIdx = Mathf.Clamp(newIdx, 0, cards.Count);
        cards.Insert(newIdx, draggedCard);
        StartCoroutine(UpdateCardPosition(dragReorderDuration));
    }

    public IEnumerator UpdateCardPosition(float duration)
    {
        if (cards.Count == 0) yield break;
        var spline = splineContainer.Spline;
        int hovIdx = hoveredCard != null ? cards.IndexOf(hoveredCard) : -1;

        for (var i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            if (card == null) continue;
            if (card == draggedCard || card == hoveredCard) continue;

            var p = GetSplineT(i);
            Vector3 splinePos = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            var rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);

            Vector3 spreadOffset = Vector3.zero;
            if (hovIdx >= 0)
                spreadOffset = i < hovIdx ? Vector3.left * hoverSpreadOffset : Vector3.right * hoverSpreadOffset;

            card.transform.DOKill();
            card.transform.DOMove(splinePos + transform.position + i * 0.01f * Vector3.back + spreadOffset, duration);
            card.transform.DORotate(rotation.eulerAngles, duration);
            card.transform.DOScale(cardScale, duration);
        }
        yield return new WaitForSeconds(duration);
    }

    private float GetSplineT(int index)
    {
        var firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2f;
        return firstCardPosition + index * cardSpacing;
    }

    private Vector3 GetSplinePositionForIndex(int index)
    {
        return (Vector3)splineContainer.Spline.EvaluatePosition(GetSplineT(index)) + transform.position;
    }

    private CardView GetCardView(Card card)
    {
        return cards.FirstOrDefault(cv => cv.Card == card);
    }
}
