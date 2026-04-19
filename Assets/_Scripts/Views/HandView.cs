using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    private readonly List<CardView> cards = new();

    public IEnumerator AddCard(CardView cardView)
    {
        cards.Add(cardView);
        yield return UpdateCardPosition(0.15f);
    }

    public CardView RemoveCard(Card card)
    {
        var cardView = GetCardView(card);
        if (cardView == null) return null;
        cards.Remove(cardView);
        StartCoroutine(UpdateCardPosition(0.15f));
        return cardView;
    }

    public IEnumerator UpdateCardPosition(float duration)
    {
        if (cards.Count == 0) yield break;
        var cardSpacing = 1f / 10;
        var firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;
        var spline = splineContainer.Spline;
        for (var i = 0; i < cards.Count; i++)
        {
            var p = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            var rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up,forward).normalized);
            cards[i].transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back, duration);
            cards[i].transform.DORotate(rotation.eulerAngles, duration);
        }
        yield return new WaitForSeconds(duration);
    }
    
    private CardView GetCardView(Card card)
    {
        return cards.Where(cardView => cardView.Card == card).FirstOrDefault();
    }
}
