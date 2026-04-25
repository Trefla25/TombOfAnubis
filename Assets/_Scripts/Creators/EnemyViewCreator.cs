using System;
using UnityEngine;

public class EnemyViewCreator : Singleton<EnemyViewCreator>
{
    [SerializeField] private EnemyView enemyViewPrefab;
    public EnemyView CreateEnemyView(EnemyData enemyData, Vector3 position, Quaternion rotation)
    {
        var enemyView = Instantiate(enemyViewPrefab, position, rotation);
        enemyView.SetUp(enemyData);
        return enemyView;
    }
}
