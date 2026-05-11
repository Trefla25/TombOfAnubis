using UnityEngine;

public class DogDiedGA : GameAction
{
    public DogView Dog { get; private set; }

    public DogDiedGA(DogView dog)
    {
        Dog = dog;
    }
}

