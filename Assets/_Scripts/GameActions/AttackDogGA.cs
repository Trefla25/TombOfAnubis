public class AttackDogGA : GameAction, IHaveCaster
{
    public EnemyView Attacker { get; private set; }
    public int Damage { get; private set; }
    public FightingView Caster {get; private set; }

    public AttackDogGA(EnemyView attacker, int damage)
    {
        Attacker = attacker;
        Damage = damage;
        Caster = attacker;
    }
}
