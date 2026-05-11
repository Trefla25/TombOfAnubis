using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSystem : Singleton<ActionSystem>
{
    private List<GameAction> reactions = null;
    public bool IsPerforming { get; private set; } = false;
    private static Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> preSubs = new();
    private static Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> postSubs = new();
    private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();

    public void Perform(GameAction action, System.Action OnPerformFinished = null)
    {
        if (IsPerforming)
        {
            Debug.LogWarning($"ActionSystem.Perform dropped {action.GetType().Name}: another action is already in progress.");
            return;
        }
        IsPerforming = true;
        StartCoroutine(Flow(action, () =>
        {
            IsPerforming = false;
            OnPerformFinished?.Invoke();
        }));
    }

    public void AddReaction(GameAction gameAction)
    {
        reactions?.Add(gameAction);
    }

    private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
    {
        reactions = action.PreReactions;
        PerformSubscribers(action, preSubs);
        yield return PerformReactions();
        
        reactions = action.PerformReactions;
        yield return PerformPerformer(action);
        yield return PerformReactions();
        
        reactions =  action.PostReactions;
        PerformSubscribers(action, postSubs);
        yield return PerformReactions();
        
        OnFlowFinished?.Invoke();
    }

    public IEnumerator PerformPerformer(GameAction action)
    {
        var type = action.GetType();
        if (performers.ContainsKey(type))
        {
            yield return performers[type](action);
        }
    }

    private void PerformSubscribers(GameAction action, Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> subs)
    {
        var type = action.GetType();
        if (subs.TryGetValue(type, out var wrappers))
        {
            foreach (var wrapper in wrappers.Values)
            {
                wrapper(action);
            }
        }
    }

    private IEnumerator PerformReactions()
    {
        foreach (var reaction in reactions)
        {
            yield return Flow(reaction);
        }
    }

    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        var type = typeof(T);
        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);
        if (performers.ContainsKey(type))
        {
            performers[type] = wrappedPerformer;
        }
        else
        {
            performers.Add(type, wrappedPerformer);
        }
    }

    public static void DetachPerformer<T>() where T : GameAction
    {
        var type = typeof(T);
        if (performers.ContainsKey(type))
        {
            performers.Remove(type);
        }
    }

    public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        var subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        if (!subs.TryGetValue(typeof(T), out var wrappers))
        {
            wrappers = new Dictionary<Delegate, Action<GameAction>>();
            subs.Add(typeof(T), wrappers);
        }
        if (wrappers.ContainsKey(reaction)) return;
        wrappers[reaction] = action => reaction((T)action);
    }

    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        var subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        if (subs.TryGetValue(typeof(T), out var wrappers))
        {
            wrappers.Remove(reaction);
        }
    }
    
}
