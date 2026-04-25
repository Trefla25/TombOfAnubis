using System.Collections.Generic;
using UnityEngine;

public class DealDamageGA : GameAction
{
    public int Amount{get; set;}
    public List<FightingView > Targets {get; set;}  

    public DealDamageGA(int amount, List<FightingView> targets)
    {
        Amount = amount;
        Targets = new(targets);
    }
}
