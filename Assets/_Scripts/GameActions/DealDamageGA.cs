using System.Collections.Generic;
using UnityEngine;

public class DealDamageGA : GameAction, IHaveCaster
{
    public int Amount{get; set;}
    public List<FightingView > Targets {get; set;}  
    public FightingView Caster { get; private set; }

    public DealDamageGA(int amount, List<FightingView> targets, FightingView caster)
    {
        Amount = amount;
        Targets = new(targets);
        Caster = caster;
    }
}
