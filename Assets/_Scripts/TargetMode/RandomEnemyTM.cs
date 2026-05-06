using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomEnemyTM : TargetMode
{
    public override List<FightingView> GetTargets()
    {
        var target = EnemySystem.Instance.Enemies[Random.Range(0, EnemySystem.Instance.Enemies.Count)];
        return new() { target };
    }
}
