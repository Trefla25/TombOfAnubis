using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DrawCardsEffect : Effect
{
    [SerializeField] private int amount;
    public override GameAction GetGameAction(List<FightingView> targets, FightingView caster)
    {
        DrawCardsGA drawCardsGA = new(amount);
        return drawCardsGA;
    }
}
