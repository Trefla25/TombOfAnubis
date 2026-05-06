using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public abstract class Effect
{
    public abstract GameAction GetGameAction(List<FightingView> targets, FightingView caster); 
}
