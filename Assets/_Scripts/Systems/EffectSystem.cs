using System.Collections;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    void OnEnable() =>
ActionSystem.AttachPerformer<PerformEffectGA>(PerformEffectPerformer);
    void OnDisable() => ActionSystem.DetachPerformer<PerformEffectGA>();

    private IEnumerator PerformEffectPerformer(PerformEffectGA performEffectGA)
    {
        FightingView caster = performEffectGA.Caster ?? DogSystem.Instance.GetAnyAliveDog();
        var effectAction = performEffectGA.Effect.GetGameAction(performEffectGA.Targets, caster);
        ActionSystem.Instance.AddReaction(effectAction);
        yield return null;
    }
}
