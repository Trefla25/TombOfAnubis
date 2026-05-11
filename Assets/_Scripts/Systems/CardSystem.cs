using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;
    [SerializeField] private Transform playedCardPoint;
    private readonly List<Card> drawPile = new();
    private readonly List<Card> discardPile = new();
    private readonly List<Card> hand = new();

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);

        ActionSystem.AttachPerformer<DiscardPlayedCardGA>(DiscardPlayedCardPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
        ActionSystem.DetachPerformer<DiscardPlayedCardGA>();
    }

    public void Setup(IEnumerable<DogView> dogs)
    {
        foreach (var dog in dogs)
        {
            foreach (var cardData in dog.Data.StartingDeck)
            {
                drawPile.Add(new Card(cardData, dog));
            }
        }
    }

    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGa)
    {
        int actualAmount = Mathf.Min(drawCardsGa.Amount, drawPile.Count);
        int nowDrawnAmount = drawCardsGa.Amount - actualAmount;
        for (var i = 0; i < actualAmount; i++) yield return DrawCard();
        if (nowDrawnAmount > 0)
        {
            RefillDeck();
            for (int i = 0; i < nowDrawnAmount; i++) yield return DrawCard();
        }
    }

    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA _)
    {
        foreach (var card in hand)
        {
            var cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        hand.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        hand.Remove(playCardGA.Card);
        var cardView = handView.RemoveCard(playCardGA.Card);
        cardView.transform.DOKill();
        cardView.transform.DOScale(playedCardPoint.localScale, 0.15f);
        var tween = cardView.transform.DOMove(playedCardPoint.position, 0.15f);
        yield return tween.WaitForCompletion();

        var owner = playCardGA.Card.OwnerDog;
        ActionSystem.Instance.AddReaction(new SpendManaGA(playCardGA.Card.Mana, owner));

        if (playCardGA.Card.ManualTargetEffects.Count > 0)
        {
            foreach (var manualTargetEffect in playCardGA.Card.ManualTargetEffects)
            {
                var ga = new PerformEffectGA(manualTargetEffect, new() { playCardGA.ManualTarget }, owner);
                ActionSystem.Instance.AddReaction(ga);
            }
        }

        foreach (var effectWrapper in playCardGA.Card.OtherEffects)
        {
            var targets = effectWrapper.TargetMode.GetTargets();
            var ga = new PerformEffectGA(effectWrapper.Effect, targets, owner);
            ActionSystem.Instance.AddReaction(ga);
        }

        discardPile.Add(playCardGA.Card);
        ActionSystem.Instance.AddReaction(new DiscardPlayedCardGA(cardView));
    }

    private IEnumerator DiscardPlayedCardPerformer(DiscardPlayedCardGA discardPlayedCardGA)
    {
        var cardView = discardPlayedCardGA.CardView;
        cardView.transform.DOKill();
        cardView.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }

    private IEnumerator DrawCard()
    {
        var card = drawPile.Draw();
        hand.Add(card);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        yield return handView.AddCard(cardView);
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        discardPile.Add(cardView.Card);
        cardView.transform.DOKill();
        cardView.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }

    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
    }
}
