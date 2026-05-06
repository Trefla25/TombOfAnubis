using UnityEngine;

public class ArmorViewUI : MonoBehaviour
{
    [SerializeField] private ArmorUI armorUI;
    [SerializeField] private Sprite armorSprite;

    public void UpdateArmorUI(int amount)
    {
        armorUI.gameObject.SetActive(amount > 0);
        if (amount > 0)
        {
            armorUI.Set(armorSprite, amount);
        }
    }
}
