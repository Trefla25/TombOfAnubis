public class DiscardPlayedCardGA : GameAction
{
    public CardView CardView { get; private set; }

    public DiscardPlayedCardGA(CardView cardView)
    {
        CardView = cardView;
    }
}
