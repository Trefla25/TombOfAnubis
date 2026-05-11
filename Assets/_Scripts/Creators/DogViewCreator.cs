using UnityEngine;

public class DogViewCreator : Singleton<DogViewCreator>
{
    [SerializeField] private DogView dogViewPrefab;

    public DogView CreateDogView(DogData data, DogRow row, DogColumn column, Vector3 position, Quaternion rotation)
    {
        var dogView = Instantiate(dogViewPrefab, position, rotation);
        dogView.SetUp(data, row, column);
        return dogView;
    }
}

