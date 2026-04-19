using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;
    private readonly List<Card> drawPile = new();
    private readonly List<Card> discardPile = new();
    private readonly List<Card> hand = new();

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }
    
    // Publics
    public void Setup(List<CardData> deckData)
    {
        foreach (var cardData in deckData)
        {
            var card = new Card(cardData);
            drawPile.Add(card);
        }
    }
    
    // Performers

    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGa)
    {
        int actualAmount = Mathf.Min(drawCardsGa.Amount, drawPile.Count);
        int nowDrawnAmount = drawCardsGa.Amount - actualAmount;

        for (var i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }

        if (nowDrawnAmount > 0)
        {
            RefillDeck();
            for (int i = 0; i < nowDrawnAmount; i++)
            {
                yield return DrawCard();
            }
        }
    }

    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA drawCardGA)
    {
        foreach (var card in hand)
        {
            discardPile.Add(card);
            var cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        
        hand.Clear();
    }
    
    // Reactions 

    private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGa)
    {
        DiscardAllCardsGA discardAllCardsGA = new();
        ActionSystem.Instance.AddReaction(discardAllCardsGA);
    }
    
    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGa)
    {
        DrawCardsGA drawCardsGA = new(3);
        ActionSystem.Instance.AddReaction(drawCardsGA);
    }
    
    //  Helpers

    private IEnumerator DrawCard()
    {
        var card = drawPile.Draw();
        hand.Add(card);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        yield return handView.AddCard((cardView));
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
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
