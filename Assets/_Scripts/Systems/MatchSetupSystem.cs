using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private PartyData party;
    [SerializeField] private PerkData perkData;
    [SerializeField] private List<EnemyData> enemyDatas;

    private void Start()
    {
        DogSystem.Instance.Setup(party);
        EnemySystem.Instance.SetUp(enemyDatas);
        CardSystem.Instance.Setup(DogSystem.Instance.Dogs);
        if (perkData != null) PerkSystem.Instance.AddPerk(new Perk(perkData));

        ActionSystem.Instance.Perform(new RefillManaGA(), () =>
        {
            ActionSystem.Instance.Perform(new DrawCardsGA(5));
        });
    }
}
