using System;
using UnityEngine;

[System.Serializable]
public class OnEnemyAttackCondition : PerkCondition
{
    public override bool SubConditionIsMet(GameAction gameAction)
    {
        // other conditions like if attacker health is > < x, etc 
        return true;
    }

    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<AttackDogGA>(reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<AttackDogGA>(reaction, reactionTiming);
    }
}
