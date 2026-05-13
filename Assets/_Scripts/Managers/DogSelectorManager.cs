 using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DogSelectorManager : MonoBehaviour
{
    [SerializeField] private DogRoster roster;
    [SerializeField] private Transform rosterContainer;
    [SerializeField] private DogSelectorCardView cardPrefab;
    [SerializeField] private Button startRunButton;
    [SerializeField] private TMP_Text selectedCountText;

    private readonly List<DogData> selected = new();
    private readonly Dictionary<DogData, DogSelectorCardView> cardByDog = new();

    void Start()
    {
        BuildRoster();
        UpdateUI();
    }

    private void BuildRoster()
    {
        if (roster == null || roster.Dogs == null)
        {
            Debug.LogError("DogSelectorManager: roster is not assigned.");
            return;
        }
        foreach (var dog in roster.Dogs)
        {
            if (dog == null) continue;
            var card = Instantiate(cardPrefab, rosterContainer);
            card.Setup(dog, OnDogClicked);
            cardByDog[dog] = card;
        }
    }

    private void OnDogClicked(DogSelectorCardView card)
    {
        var dog = card.Dog;
        if (selected.Contains(dog))
        {
            selected.Remove(dog);
            card.SetSelected(false);
        }
        else
        {
            if (selected.Count >= PartySelection.PartySize) return;
            selected.Add(dog);
            card.SetSelected(true);
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (selectedCountText != null)
            selectedCountText.text = "Selected: " + selected.Count + "/" + PartySelection.PartySize;
        if (startRunButton != null)
            startRunButton.interactable = selected.Count == PartySelection.PartySize;
    }

    public void StartRun()
    {
        if (selected.Count != PartySelection.PartySize) return;

        if (PartySelection.Instance == null)
        {
            Debug.LogError("DogSelectorManager.StartRun: PartySelection singleton not found. Did you boot through the Core scene?");
            return;
        }
        PartySelection.Instance.SetSelection(selected);

        if (SceneController.Instance == null)
        {
            Debug.LogWarning("DogSelectorManager.StartRun: SceneController not found. Falling back to direct load.");
            SceneManager.LoadScene(SceneDatabase.Scenes.Match);
            return;
        }

        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.Match, setActive: true)
            .WithOverlay()
            .Perform();
    }
    public void EndSession()
    {
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Menu, SceneDatabase.Scenes.MainMenu, setActive: true)
            .Unload(SceneDatabase.Slots.Session)
            .Unload(SceneDatabase.Slots.SessionContent)
            .WithClearUnusedAssets()
            .WithOverlay()
            .Perform();
    }

    public void SwitchToMatch()
    {
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.Match, setActive: true)
            .WithOverlay()
            .Perform();
    }
}
