using System;
using UnityEngine;

public class DogSystem : Singleton<DogSystem>
{
    [field: SerializeField] public DogsView DogsView { get; private set; }

    public void Setup(DogsData dogsData)
    {
        DogsView.SetUp(dogsData);
    }

    private void OnEnable()
    {                                                                                                                                                               
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        if (DogsView.CurrentArmor > 0)
            DogsView.LoseArmor(DogsView.CurrentArmor);
    }
}
