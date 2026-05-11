using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class DogTM : TargetMode
{
    public override List<FightingView> GetTargets()
    {
        return DogSystem.Instance.AliveDogs.Cast<FightingView>().ToList();
    }
}