using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dog Roster")]
public class DogRoster : ScriptableObject
{
    [field: SerializeField] public List<DogData> Dogs { get; private set; } = new();
}

