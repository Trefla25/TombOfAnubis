using System.Collections.Generic;
using UnityEngine;

  [CreateAssetMenu(menuName = "Data/Party")]
public class PartyData : ScriptableObject
{
    [field: SerializeField] public List<DogData> Dogs { get; private set; } = new();
}
