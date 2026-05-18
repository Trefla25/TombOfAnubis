using System.Collections.Generic;

public class RunState
  {
      public List<DogData> Dogs { get; }
      public Dictionary<DogData, int> CurrentHp { get; }
      public Dictionary<DogData, List<CardData>> Decks { get; }
      public List<PerkData> Perks { get; }
      public int Gold { get; set; }
      public int Floor { get; set; }
      public int Seed { get; }

      public RunState(IEnumerable<DogData> dogs, int seed, int startingGold)
      {
          Dogs = new List<DogData>(dogs);
          CurrentHp = new();
          Decks = new();
          Perks = new();
          Gold = startingGold;
          Floor = 1;
          Seed = seed;

          foreach (var dog in Dogs)
          {
              CurrentHp[dog] = dog.Health;
              Decks[dog] = new List<CardData>(dog.StartingDeck);
          }
      }
  }