  using TMPro;
  using UnityEngine;

  public class DogView : FightingView
  {
    [SerializeField] private TMP_Text manaText;

    public DogData Data { get; private set; }
    public DogRow Row { get; private set; }
    public DogColumn Column { get; private set; }
    public int CurrentMana { get; private set; }
    public int MaxMana => Data != null ? Data.ManaPerTurn : 0;
    public bool IsAlive => CurrentHealth > 0;

    public void SetUp(DogData data, DogRow row, DogColumn column)
    {
        Data = data;
        Row = row;
        Column = column;
        CurrentMana = data.ManaPerTurn;
        SetUpBase(data.Health, data.Image);
        UpdateManaText();
    }

    public void RefillMana()
    {
        CurrentMana = MaxMana;
        UpdateManaText();
    }

    public bool TrySpendMana(int amount)
    {
        if (CurrentMana < amount) return false;
        CurrentMana -= amount;
        UpdateManaText();
        return true;
    }

    public bool HasEnoughMana(int amount) => CurrentMana >= amount;

    private void UpdateManaText()
    {
        if (manaText != null) manaText.text = CurrentMana.ToString();
    }
  }