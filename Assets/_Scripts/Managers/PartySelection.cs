using System.Collections.Generic;
using System.Linq;

public class PartySelection : PersistentSingleton<PartySelection>
{
    public const int PartySize = 4;

    public List<DogData> SelectedDogs { get; private set; } = new();
    public bool HasSelection => SelectedDogs.Count == PartySize;

    public void SetSelection(IEnumerable<DogData> dogs)
    {
        SelectedDogs = dogs?.ToList() ?? new List<DogData>();
    }

    public void Clear()
    {
        SelectedDogs.Clear();
    }
}