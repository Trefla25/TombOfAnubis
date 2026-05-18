using System.Collections.Generic;
using UnityEngine;

public class DogBoardView : MonoBehaviour
{
    [Tooltip("Order: 0=FrontLeft, 1=FrontRight, 2=BackLeft, 3=BackRight")]
    [SerializeField] private List<Transform> slots;

    public List<DogView> DogViews { get; private set; } = new();

    public void AddDog(DogData data, DogRow row, DogColumn column, int startingHp)
    {
        int slotIndex = SlotIndex(row, column);
        Transform slot = slots[slotIndex];
        var dogView = DogViewCreator.Instance.CreateDogView(data, row, column, startingHp, slot.position, slot.rotation);
        dogView.transform.parent = slot;
        DogViews.Add(dogView);
    }

    public DogView GetDogAt(DogRow row, DogColumn column)
    {
        foreach (var d in DogViews)
            if (d.Row == row && d.Column == column) return d;
        return null;
    }

    private static int SlotIndex(DogRow row, DogColumn col)
    {
        int rowOffset = row == DogRow.Front ? 0 : 2;
        int colOffset = col == DogColumn.Left ? 0 : 1;
        return rowOffset + colOffset;
    }
}
