using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class DogTM : TargetMode
{
    public override List<FightingView> GetTargets(FightingView caster)
    {
        return DogSystem.Instance.AliveDogs.Cast<FightingView>().ToList();
    }
}