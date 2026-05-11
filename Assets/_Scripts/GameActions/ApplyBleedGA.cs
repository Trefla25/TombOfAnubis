using UnityEngine;

public class ApplyBleedGA : GameAction
{
    public int BleedDamage {get; private set;}
    public FightingView Target{get; private set;}

    public ApplyBleedGA(int bleedDamage, FightingView target)
    {
        BleedDamage = bleedDamage;
        Target = target;
    }
}
