using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private DogsData dogsData;

    [SerializeField] private List<EnemyData> enemyDatas;

    private void Start()
    {
        DogSystem.Instance.Setup(dogsData);
        EnemySystem.Instance.SetUp(enemyDatas);
        CardSystem.Instance.Setup(dogsData.Deck);
        RefillManaGA refillManaGA = new();
        ActionSystem.Instance.Perform(refillManaGA, () =>
        {
            DrawCardsGA drawCardsGA = new(3);
            ActionSystem.Instance.Perform(drawCardsGA);
        });
    }
}
