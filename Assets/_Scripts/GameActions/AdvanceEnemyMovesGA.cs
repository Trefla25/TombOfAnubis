using System.Collections.Generic;

public class AdvanceEnemyMovesGA : GameAction
{
    public List<EnemyView> Enemies { get; private set; }

    public AdvanceEnemyMovesGA(List<EnemyView> enemies)
    {
        Enemies = enemies;
    }
}
