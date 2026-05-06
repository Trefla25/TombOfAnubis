using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GainArmorEffect : Effect
{
    [SerializeField] private int armorAmount;
    public override GameAction GetGameAction(List<FightingView> targets, FightingView caster)
    {
         return new GainArmorGA(armorAmount, targets);
    }
}
