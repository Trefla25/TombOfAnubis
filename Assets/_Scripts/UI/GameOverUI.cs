using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject lostPanel;

    void Awake()
    {
        if (lostPanel != null) lostPanel.SetActive(false);
    }

    void OnEnable()
    {
        ActionSystem.SubscribeReaction<RunLostGA>(OnRunLost, ReactionTiming.POST);
    }

    void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<RunLostGA>(OnRunLost, ReactionTiming.POST);
    }

    private void OnRunLost(RunLostGA _)
    {
        if (lostPanel != null) lostPanel.SetActive(true);
    }
}
