using System.Collections.Generic;
using UnityEngine;

public class AddStatusEffectGA : GameAction
{
    public StatusEffectType StatusEffectType { get; private set; }
    public int StackCount { get; private set; }
    public List<FightingView> Targets { get; private set; }
    public AddStatusEffectGA(StatusEffectType statusEffectType, int stackCount, List<FightingView> targets)
    {
        StatusEffectType = statusEffectType;
        StackCount = stackCount;
        Targets = targets;
    }
}
