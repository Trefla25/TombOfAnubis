public class AttackDogGA : GameAction
{
    public EnemyView Attacker { get; private set; }
    public int Damage { get; private set; }

    public AttackDogGA(EnemyView attacker, int damage)
    {
        Attacker = attacker;
        Damage = damage;
    }
}
