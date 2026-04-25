using DG.Tweening;
using UnityEngine;

public class CardViewHoverSystem : Singleton<CardViewHoverSystem>
{
    [SerializeField] private CardView cardViewHover;
    [SerializeField] private float animDuration = 0.2f;
    [SerializeField] private Vector3 hoverScale = Vector3.one;

    protected override void Awake()
    {
        base.Awake(); // if needed
        cardViewHover.gameObject.SetActive(false);
        cardViewHover.transform.localScale = Vector3.zero;
    }

    public void Show(Card card, Vector3 position)
    {
        cardViewHover.gameObject.SetActive(true);
        cardViewHover.Setup(card);
        cardViewHover.transform.position = position;

        cardViewHover.transform.DOKill();
        cardViewHover.transform.DOScale(hoverScale, animDuration).SetEase(Ease.OutBack);
    }

    public void Hide()
    {
        cardViewHover.transform.DOKill();
        cardViewHover.transform.DOScale(Vector3.zero, animDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() => cardViewHover.gameObject.SetActive(false));
    }
}