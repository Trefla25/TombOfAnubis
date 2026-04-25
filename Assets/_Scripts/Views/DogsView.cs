using UnityEngine;

public class DogsView : FightingView
{
    public void SetUp(DogsData dogsData)
    {
        SetUpBase(dogsData.Health, dogsData.Image);
    }
}
