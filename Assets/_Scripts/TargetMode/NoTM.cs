using System.Collections.Generic;

[System.Serializable]
public class NoTM : TargetMode
{
    public override List<FightingView> GetTargets(FightingView caster)
    {
        return null;
    }
}