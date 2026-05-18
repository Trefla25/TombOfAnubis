using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RunController : PersistentSingleton<RunController>
{
    [SerializeField] private RunConfig config;

    public RunConfig Config => config;
    public RunState CurrentRun { get; private set; }
    public bool HasRun => CurrentRun != null;

    public void StartNewRun(IEnumerable<DogData> dogs)
    {
        int seed = System.Environment.TickCount;
        Random.InitState(seed);
        int startingGold = config != null ? config.StartingGold : 0;
        CurrentRun = new RunState(dogs, seed, startingGold);
        int totalCards = CurrentRun.Decks.Values.Sum(d => d.Count);
        Debug.Log($"[RunController] New run started. Seed={seed}, Dogs={CurrentRun.Dogs.Count}, TotalCards={totalCards}");
    }

    public void EndRun()
    {
        CurrentRun = null;
    }
}