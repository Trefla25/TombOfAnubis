using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView : FightingView
{
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private Image actionImage;

    private List<MoveData> _moveset;
    private int _moveIndex;

    public MoveData CurrentMove => _moveset[_moveIndex];

    public void SetUp(EnemyData enemyData)
    {
        _moveset = enemyData.Moveset;
        _moveIndex = 0;
        UpdateMoveDisplay();
        SetUpBase(enemyData.Health, enemyData.Image);
    }

    public void AdvanceMove()
    {
        _moveIndex = (_moveIndex + 1) % _moveset.Count;
        UpdateMoveDisplay();
    }

    private void UpdateMoveDisplay()
    {
        var move = CurrentMove;
        bool isAttack = move.Type == MoveType.Attack;
        attackText.enabled = isAttack;
        attackText.text = isAttack ? $"{move.Damage}" : "";
        if (actionImage != null)
        {
            actionImage.sprite = move.MoveImage;
            actionImage.color = Color.white;
        }
    }

    public void SetIntentVisible(bool visible)
    {
        if (attackText != null) attackText.enabled = visible && CurrentMove.Type == MoveType.Attack;
        if (actionImage != null) actionImage.enabled = visible;
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
