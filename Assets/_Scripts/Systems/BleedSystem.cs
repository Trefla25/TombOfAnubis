using System;
using System.Collections;
using UnityEngine;

public class BleedSystem : MonoBehaviour
{
    [SerializeField] private GameObject bleedVFX;
    
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<ApplyBleedGA>(ApplyBleedPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<ApplyBleedGA>();
    }

    private IEnumerator ApplyBleedPerformer(ApplyBleedGA applyBleedGA)
    {
        var target = applyBleedGA.Target;
        Instantiate(bleedVFX, target.SpritePosition, Quaternion.identity);
        target.Damage(applyBleedGA.BleedDamage);
        target.RemoveStatusEffect(StatusEffectType.BLEED, 1);
        if (target.CurrentHealth <= 0 && target is EnemyView enemyView)
            ActionSystem.Instance.AddReaction(new KillEnemyGA(enemyView));
        yield return new WaitForSeconds(1f);
    }
}
