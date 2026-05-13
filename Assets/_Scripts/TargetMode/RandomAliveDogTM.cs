using System.Collections.Generic;

[System.Serializable]
public class RandomAliveDogTM : TargetMode
{
    public override List<FightingView> GetTargets(FightingView caster)
    {
        var dog = DogSystem.Instance.GetRandomAliveDog();
        return dog != null ? new() { dog } : new();
    }
}