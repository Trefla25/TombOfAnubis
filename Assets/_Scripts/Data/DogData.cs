using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dog")]
public class DogData : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public DogBreed Breed { get; private set; }
    [field: SerializeField] public DogClass Class { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public Color TintColor { get; private set; } = Color.white;
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int ManaPerTurn { get; private set; } = 2;
    [field: SerializeField] public List<CardData> StartingDeck { get; private set; } = new();
}
