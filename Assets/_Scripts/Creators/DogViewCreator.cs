using UnityEngine;

public class DogViewCreator : Singleton<DogViewCreator>
{
    [SerializeField] private DogView dogViewPrefab;

    public DogView CreateDogView(DogData data, DogRow row, DogColumn column, int startingHp, Vector3 position, Quaternion rotation)
    {
        var dogView = Instantiate(dogViewPrefab, position, rotation);
        dogView.SetUp(data, row, column, startingHp);
        return dogView;
    }
}

