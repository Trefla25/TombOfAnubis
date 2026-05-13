using DG.Tweening;
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
    public bool IsDead { get; private set; }
    public bool IsAlive => !IsDead && CurrentHealth > 0;

    private static readonly Color DeadTint = new(0.35f, 0.35f, 0.35f, 0.8f);

    public void SetUp(DogData data, DogRow row, DogColumn column)
    {
        Data = data;
        Row = row;
        Column = column;
        CurrentMana = data.ManaPerTurn;
        IsDead = false;
        SetUpBase(data.Health, data.Image);
        UpdateManaText();
    }

    public void RefillMana()
    {
        if (IsDead) return;
        CurrentMana = MaxMana;
        UpdateManaText();
    }

    public bool TrySpendMana(int amount)
    {
        if (IsDead || CurrentMana < amount) return false;
        CurrentMana -= amount;
        UpdateManaText();
        return true;
    }

    public bool HasEnoughMana(int amount) => !IsDead && CurrentMana >= amount;

    public void MarkDead()
    {
        if (IsDead) return;
        IsDead = true;
        CurrentMana = 0;
        if (manaText != null) manaText.text = "X";
        if (SpriteRenderer != null) SpriteRenderer.DOColor(DeadTint, 0.3f);
        transform.DOScale(transform.localScale * 0.85f, 0.3f);
    }

    private void UpdateManaText()
    {
        if (manaText != null) manaText.text = CurrentMana.ToString();
    }

    private void OnDestroy()
    {
        transform.DOKill();
        if (SpriteRenderer != null) SpriteRenderer.DOKill();
    }
}
