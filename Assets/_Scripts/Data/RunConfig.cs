using UnityEngine;

[CreateAssetMenu(menuName = "Data/Run Config")]
public class RunConfig : ScriptableObject
{
    [field: SerializeField] public int HandSize { get; private set; } = 5;
    [field: SerializeField] public int DrawPerTurn { get; private set; } = 5;
    [field: SerializeField] public int ManaPerDogPerTurn { get; private set; } = 2;
    [field: SerializeField] public int StartingGold { get; private set; } = 0;
    [field: SerializeField] public int RevivalHp { get; private set; } = 1;
}