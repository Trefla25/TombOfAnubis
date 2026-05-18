using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    [SerializeField] private EnemyBoardView enemyBoardView;
    public List<EnemyView> Enemies => enemyBoardView.EnemyViews;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<AttackDogGA>(AttackDogPerformer);
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer);

        ActionSystem.AttachPerformer<AdvanceEnemyMovesGA>(AdvanceEnemyMovesPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<AttackDogGA>();
        ActionSystem.DetachPerformer<KillEnemyGA>();
        ActionSystem.DetachPerformer<AdvanceEnemyMovesGA>();
    }

    public void SetUp(List<EnemyData> enemyDatas)
    {
        foreach (var enemyData in enemyDatas)
            enemyBoardView.AddEnemy(enemyData);
    }

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        foreach (var enemy in enemyBoardView.EnemyViews)
        {
            int bleedStacks = enemy.GetStatusEffectStacks(StatusEffectType.BLEED);
            if (bleedStacks > 0)
                ActionSystem.Instance.AddReaction(new ApplyBleedGA(bleedStacks, enemy));
        }
        foreach (var enemy in enemyBoardView.EnemyViews)
        {
            var move = enemy.CurrentMove;
            if (move.Type != MoveType.Attack) continue;
            var target = DogSystem.Instance.GetRandomAliveDog();
            if (target == null) continue;
            ActionSystem.Instance.AddReaction(new AttackDogGA(enemy, move.Damage, target));
        }
        ActionSystem.Instance.AddReaction(new AdvanceEnemyMovesGA(enemyBoardView.EnemyViews));
        yield return null;
    }

    private IEnumerator AttackDogPerformer(AttackDogGA attackDogGA)
    {
        var attacker = attackDogGA.Attacker;
        if (attacker == null) yield break;
        var target = attackDogGA.Target;
        if (target == null || !target.IsAlive)
        {
            target = DogSystem.Instance.GetRandomAliveDog();
            if (target == null) yield break;
        }
        var tween = attacker.transform.DOMoveX(attacker.transform.position.x - 1f, 0.15f);
        yield return tween.WaitForCompletion();
        ActionSystem.Instance.AddReaction(new DealDamageGA(attackDogGA.Damage, new() { target }, attackDogGA.Caster));
        var returnTween = attacker.transform.DOMoveX(attacker.transform.position.x + 1f, 0.25f);
        yield return returnTween.WaitForCompletion();
    }

    private IEnumerator AdvanceEnemyMovesPerformer(AdvanceEnemyMovesGA advanceEnemyMovesGA)
    {
        foreach (var enemy in advanceEnemyMovesGA.Enemies)
        {
            if (enemy == null) continue;
            enemy.AdvanceMove();
        }
        yield return null;
    }

    private IEnumerator KillEnemyPerformer(KillEnemyGA killEnemyGA)
    {
        yield return enemyBoardView.RemoveEnemy(killEnemyGA.EnemyView);
        if (Enemies.Count == 0 && DogSystem.Instance.AliveCount > 0)
        {
            ActionSystem.Instance.AddReaction(new CombatEndedGA());
        }
    }
}
