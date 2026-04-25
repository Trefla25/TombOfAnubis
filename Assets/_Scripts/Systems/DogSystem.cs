using System;
using UnityEngine;

public class DogSystem : Singleton<DogSystem>
{
    [field: SerializeField] public DogsView DogsView { get; private set; }

    public void Setup(DogsData dogsData)
    {
        DogsView.SetUp(dogsData);
    }
}
