using System;
using UnityEngine;

[Serializable]
public class DrawCardsEffect : Effect
{
    [SerializeField] private int amount;
    public override GameAction GetGameAction()
    {
        DrawCardsGA drawCardsGA = new(amount);
        return drawCardsGA;
    }
}
