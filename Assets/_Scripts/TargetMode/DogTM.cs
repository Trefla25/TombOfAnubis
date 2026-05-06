using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogTM : TargetMode
{
    public override List<FightingView> GetTargets()
    {
        List<FightingView> targets = new()
        {
            DogSystem.Instance.DogsView
        };
        return targets;
    }
}
