using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmorUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text armorText;

    public void Set(Sprite sprite, int amount)
    {
        image.sprite = sprite;
        armorText.text = amount.ToString();
    }
}
