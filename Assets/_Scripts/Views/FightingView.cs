using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FightingView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private StatusEffectsUI statusEffectsUI;
    [SerializeField] private ArmorViewUI armorViewUI;
    public Vector3 SpritePosition => spriteRenderer.bounds.center;
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int CurrentArmor { get; private set; }
    private Dictionary<StatusEffectType, int> statusEffects = new();

    protected void SetUpBase(int health, Sprite image)
    {
        MaxHealth = CurrentHealth = health;
        CurrentArmor = 0;
        armorViewUI?.UpdateArmorUI(0);
        spriteRenderer.sprite = image;
        UpdateHealthText();
    }

    public void GainArmor(int amount)
    {
        CurrentArmor += amount;
        armorViewUI?.UpdateArmorUI(CurrentArmor);
    }

    public void LoseArmor(int amount)
    {
        CurrentArmor = Mathf.Max(0, CurrentArmor - amount);
        armorViewUI?.UpdateArmorUI(CurrentArmor);
    }

    private void UpdateHealthText()
    {
        healthText.text = CurrentHealth + "/" + MaxHealth;
    }

    public void Damage(int damageAmount)
    {
        int absorbed = Mathf.Min(CurrentArmor, damageAmount);
        CurrentArmor -= absorbed;
        armorViewUI?.UpdateArmorUI(CurrentArmor);

        int healthDamage = damageAmount - absorbed;
        CurrentHealth = Mathf.Max(0, CurrentHealth - healthDamage);

        transform.DOShakePosition(0.2f, 0.5f);
        UpdateHealthText();
    }

    public void AddStatusEffect(StatusEffectType type, int stackCount)
    {
        if (statusEffects.ContainsKey(type))
        {
            statusEffects[type] += stackCount;
        }
        else
        {
            statusEffects.Add(type, stackCount);
        }

        statusEffectsUI.UpdateStatusEffectUI(type, GetStatusEffectStacks(type));
    }

    public void RemoveStatusEffect(StatusEffectType type, int stackCount)
    {
        if (statusEffects.ContainsKey(type))
        {
            statusEffects[type] -= stackCount;
            if (statusEffects[type] <= 0)
            {
                statusEffects.Remove(type);
            }
            statusEffectsUI.UpdateStatusEffectUI(type, GetStatusEffectStacks(type)); 
        }
    }

    public int GetStatusEffectStacks(StatusEffectType type)
    {
        if(statusEffects.ContainsKey(type))
        {
            return statusEffects[type];
        }
        else
        {
            return 0;
        }
    }
}
