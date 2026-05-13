using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.UI;

  public class PositioningSystem : Singleton<PositioningSystem>
  {
      [SerializeField] private GameObject overlayRoot;
      [SerializeField] private Transform tokenContainer;
      [SerializeField] private PositioningTokenView tokenPrefab;
      [SerializeField] private List<PositioningSlotView> slots;
      [SerializeField] private Button startCombatButton;

      private readonly Dictionary<DogData, PositioningTokenView> tokenByDog = new();
      private PositioningTokenView selectedToken;

      public bool IsPositioning { get; private set; }

      public void BeginPositioning(IList<DogData> dogs)
      {
          if (overlayRoot != null) overlayRoot.SetActive(true);
          IsPositioning = true;

          for (int i = 0; i < slots.Count; i++)
          {
              var (row, col) = SlotIndexToCoords(i);
              slots[i].Setup(row, col, OnSlotClicked);
          }

          ClearTokens();
          foreach (var dog in dogs)
          {
              var token = Instantiate(tokenPrefab, tokenContainer);
              token.Setup(dog, OnTokenClicked);
              tokenByDog[dog] = token;
          }

          if (startCombatButton != null) startCombatButton.interactable = false;
      }

      public IEnumerable<(DogData dog, DogRow row, DogColumn col)> GetAssignments()
      {
          foreach (var slot in slots)
          {
              if (slot.AssignedDog != null)
                  yield return (slot.AssignedDog, slot.Row, slot.Column);
          }
      }

      public void StartCombat()
      {
          if (!AllSlotsFilled()) return;
          if (overlayRoot != null) overlayRoot.SetActive(false);
          IsPositioning = false;
          ActionSystem.Instance.Perform(new StartCombatGA());
      }

      private void OnTokenClicked(PositioningTokenView token)
      {
          if (selectedToken == token)
          {
              selectedToken.SetHighlight(false);
              selectedToken = null;
              return;
          }
          if (selectedToken != null) selectedToken.SetHighlight(false);
          selectedToken = token;
          selectedToken.SetHighlight(true);
      }

      private void OnSlotClicked(PositioningSlotView slot)
      {
          if (selectedToken != null)
          {
              if (!slot.IsEmpty)
              {
                  var displaced = slot.AssignedDog;
                  slot.ClearAssignment();
                  if (tokenByDog.TryGetValue(displaced, out var displacedToken))
                      displacedToken.SetPlaced(false);
              }
              slot.Assign(selectedToken.Dog);
              selectedToken.SetPlaced(true);
              selectedToken.SetHighlight(false);
              selectedToken = null;
          }
          else if (!slot.IsEmpty)
          {
              var dog = slot.AssignedDog;
              slot.ClearAssignment();
              if (tokenByDog.TryGetValue(dog, out var token))
                  token.SetPlaced(false);
          }

          if (startCombatButton != null)
              startCombatButton.interactable = AllSlotsFilled();
      }

      private bool AllSlotsFilled()
      {
          foreach (var slot in slots) if (slot.IsEmpty) return false;
          return true;
      }

      private void ClearTokens()
      {
          foreach (var kvp in tokenByDog) if (kvp.Value != null) Destroy(kvp.Value.gameObject);
          tokenByDog.Clear();
          selectedToken = null;
      }

      private static (DogRow, DogColumn) SlotIndexToCoords(int i) => i switch
      {
          0 => (DogRow.Front, DogColumn.Left),
          1 => (DogRow.Front, DogColumn.Right),
          2 => (DogRow.Back,  DogColumn.Left),
          _ => (DogRow.Back,  DogColumn.Right),
      };
  }