public class SpendManaGA : GameAction
{
    public int Amount { get; private set; }
    public DogView Spender { get; private set; }

    public SpendManaGA(int amount, DogView spender)
    {
        Amount = amount;
        Spender = spender;
    }
}