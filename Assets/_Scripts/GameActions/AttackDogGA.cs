public class AttackDogGA : GameAction, IHaveCaster
{
    public EnemyView Attacker { get; private set; }
    public int Damage { get; private set; }
    public DogView Target { get; private set; }
    public FightingView Caster { get; private set; }

    public AttackDogGA(EnemyView attacker, int damage, DogView target)
    {
        Attacker = attacker;
        Damage = damage;
        Target = target;
        Caster = attacker;
    }
}