using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [Tooltip("Used only when running MatchScene directly without booting through DogSelector.")]
    [SerializeField] private PartyData fallbackParty;
    [SerializeField] private PerkData perkData;
    [SerializeField] private List<EnemyData> enemyDatas;

    private List<DogData> partyDogs;

    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<StartCombatGA>(OnStartCombatPost, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<StartCombatGA>(OnStartCombatPost, ReactionTiming.POST);
    }

    private void Start()
    {
        partyDogs = ResolveParty();
        if (partyDogs == null || partyDogs.Count == 0)
        {
            Debug.LogError("MatchSetupSystem: no party available.");
            return;
        }

        EnemySystem.Instance.SetUp(enemyDatas);
        SetEnemyIntentsVisible(false);

        if (PositioningSystem.Instance != null)
        {
            PositioningSystem.Instance.BeginPositioning(partyDogs);
        }
        else
        {
            Debug.LogWarning("PositioningSystem missing — using default positions.");
            BeginCombatWithDefaultPositions();
        }
    }

    private List<DogData> ResolveParty()
    {
        if (PartySelection.Instance != null && PartySelection.Instance.HasSelection)
            return PartySelection.Instance.SelectedDogs;
        return fallbackParty != null ? fallbackParty.Dogs.ToList() : null;
    }

    private void OnStartCombatPost(StartCombatGA _)
    {
        var assignments = PositioningSystem.Instance.GetAssignments().ToList();
        DogSystem.Instance.Setup(assignments);
        CardSystem.Instance.Setup(DogSystem.Instance.Dogs);
        if (perkData != null) PerkSystem.Instance.AddPerk(new Perk(perkData));

        SetEnemyIntentsVisible(true);
        ActionSystem.Instance.AddReaction(new RefillManaGA());
        ActionSystem.Instance.AddReaction(new DrawCardsGA(5));
    }

    private void BeginCombatWithDefaultPositions()
    {
        DogSystem.Instance.Setup(partyDogs);
        CardSystem.Instance.Setup(DogSystem.Instance.Dogs);
        if (perkData != null) PerkSystem.Instance.AddPerk(new Perk(perkData));
        SetEnemyIntentsVisible(true);
        ActionSystem.Instance.Perform(new RefillManaGA(), () =>
        {
            ActionSystem.Instance.Perform(new DrawCardsGA(5));
        });
    }

    private void SetEnemyIntentsVisible(bool visible)
    {
        if (EnemySystem.Instance == null) return;
        foreach (var enemy in EnemySystem.Instance.Enemies)
        {
            if (enemy != null) enemy.SetIntentVisible(visible);
        }
    }
}