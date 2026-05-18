using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;

    void Awake()
    {
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    void OnEnable()
    {
        ActionSystem.SubscribeReaction<CombatEndedGA>(OnCombatEnded, ReactionTiming.POST);
    }

    void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<CombatEndedGA>(OnCombatEnded, ReactionTiming.POST);
    }

    public void OnNextCombatClicked()
    {
        if (ActionSystem.Instance != null) ActionSystem.Instance.Clear();
        DOTween.KillAll();
        
        if (SceneController.Instance == null)
        {
            Debug.LogWarning("VictoryUI: SceneController not found. Falling back to direct load.");
            SceneManager.LoadScene(SceneDatabase.Scenes.Match);
            return;
        }
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.Match, setActive: true)
            .WithOverlay()
            .Perform();
    }

    private void OnCombatEnded(CombatEndedGA _)
    {
        if (DogSystem.Instance == null || DogSystem.Instance.AliveCount == 0) return;
        if (victoryPanel != null) victoryPanel.SetActive(true);
    }
}