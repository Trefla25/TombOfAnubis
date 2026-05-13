using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class LowestHpDogTM : TargetMode
{
    public override List<FightingView> GetTargets(FightingView caster)
    {
        var dog = DogSystem.Instance.AliveDogs
            .OrderBy(d => d.CurrentHealth)
            .FirstOrDefault();
        return dog != null ? new() { dog } : new();
    }
}