using System.Collections.Generic;

[System.Serializable]
public class OwnerDogTM : TargetMode
{
    public override List<FightingView> GetTargets(FightingView caster)
    {
        if (caster is DogView dog && dog.IsAlive)
            return new() { dog };
        return new();
    }
}