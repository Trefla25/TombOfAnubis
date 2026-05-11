using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DogSystem : Singleton<DogSystem>
{
    [SerializeField] private DogBoardView dogBoardView;

    public IReadOnlyList<DogView> Dogs => dogBoardView.DogViews;
    public IEnumerable<DogView> AliveDogs => dogBoardView.DogViews.Where(d => d.IsAlive);
    public int AliveCount => AliveDogs.Count();

    public void Setup(PartyData party)
    {
        if (party == null || party.Dogs == null || party.Dogs.Count == 0)
        {
            Debug.LogError("DogSystem.Setup called with empty PartyData");
            return;
        }
        for (int i = 0; i < party.Dogs.Count; i++)
        {
            var (row, col) = DefaultSlotForIndex(i);
            dogBoardView.AddDog(party.Dogs[i], row, col);
        }
    }

    public DogView GetRandomAliveDog()
    {
        var alive = AliveDogs.ToList();
        return alive.Count == 0 ? null : alive[Random.Range(0, alive.Count)];
    }

    public DogView GetAnyAliveDog() => AliveDogs.FirstOrDefault();

    private static (DogRow, DogColumn) DefaultSlotForIndex(int i) => i switch
    {
        0 => (DogRow.Front, DogColumn.Left),
        1 => (DogRow.Front, DogColumn.Right),
        2 => (DogRow.Back,  DogColumn.Left),
        _ => (DogRow.Back,  DogColumn.Right),
    };

    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
        ActionSystem.SubscribeReaction<DealDamageGA>(DealDamagePostReaction, ReactionTiming.POST);
        ActionSystem.AttachPerformer<DogDiedGA>(DogDiedPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
        ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamagePostReaction, ReactionTiming.POST);
        ActionSystem.DetachPerformer<DogDiedGA>();
    }

    private void EnemyTurnPreReaction(EnemyTurnGA _)
    {
        ActionSystem.Instance.AddReaction(new DiscardAllCardsGA());
    }

    private void EnemyTurnPostReaction(EnemyTurnGA _)
    {
        foreach (var dog in AliveDogs)
        {
            if (dog.CurrentArmor > 0) dog.LoseArmor(dog.CurrentArmor);

            int bleedStacks = dog.GetStatusEffectStacks(StatusEffectType.BLEED);
            if (bleedStacks > 0)
                ActionSystem.Instance.AddReaction(new ApplyBleedGA(bleedStacks, dog));
        }
        ActionSystem.Instance.AddReaction(new DrawCardsGA(5));
    }

    private void DealDamagePostReaction(DealDamageGA dealDamageGA)
    {
        if (dealDamageGA.Targets == null) return;
        foreach (var target in dealDamageGA.Targets)
        {
            if (target is DogView dog && dog.CurrentHealth <= 0 && !dog.IsDead)
            {
                ActionSystem.Instance.AddReaction(new DogDiedGA(dog));
            }
        }
    }

    private IEnumerator DogDiedPerformer(DogDiedGA dogDiedGA)
    {
        dogDiedGA.Dog.MarkDead();
        yield return new WaitForSeconds(0.35f);
        if (AliveCount == 0)
        {
            ActionSystem.Instance.AddReaction(new RunLostGA());
        }
    }
}
