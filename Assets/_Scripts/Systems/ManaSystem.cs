 using System.Collections;
  using UnityEngine;

  public class ManaSystem : Singleton<ManaSystem>
  {
      void OnEnable()
      {
          ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer);
          ActionSystem.AttachPerformer<RefillManaGA>(RefillManaPerformer);
          ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
      }

      void OnDisable()
      {
          ActionSystem.DetachPerformer<SpendManaGA>();
          ActionSystem.DetachPerformer<RefillManaGA>();
          ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
      }

      public bool HasEnoughMana(int amount, DogView spender)
      {
          return spender != null && spender.IsAlive && spender.HasEnoughMana(amount);
      }

      private IEnumerator SpendManaPerformer(SpendManaGA spendManaGA)
      {
          spendManaGA.Spender?.TrySpendMana(spendManaGA.Amount);
          yield return null;
      }

      private IEnumerator RefillManaPerformer(RefillManaGA refillManaGA)
      {
          foreach (var dog in DogSystem.Instance.AliveDogs)
              dog.RefillMana();
          yield return null;
      }

      private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
      {
          ActionSystem.Instance.AddReaction(new RefillManaGA());
      }
  }