using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllEnemiesTargetMode : TargetMode
{
    public override List<FightingView> GetTargets()
    {
        return new(EnemySystem.Instance.Enemies);
    }
}
