using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomEnemyTM : TargetMode
{
    public override List<FightingView> GetTargets(FightingView caster)
    {
        var enemies = EnemySystem.Instance.Enemies;
        if (enemies == null || enemies.Count == 0) return new();
        var target = enemies[Random.Range(0, enemies.Count)];
        return new() { target };
    }
}