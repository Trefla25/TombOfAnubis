using System.Collections.Generic;
using UnityEngine;

public class GainArmorGA : GameAction
{
    public int ArmorAmount { get; private set; }
    public List<FightingView> Targets { get; private set; }
    public GainArmorGA(int armorAmount, List<FightingView> targets)
    {
        ArmorAmount = armorAmount;
        Targets = targets;
    }
}
