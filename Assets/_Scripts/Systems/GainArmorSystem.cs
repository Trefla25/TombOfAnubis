using System.Collections;
using UnityEngine;

public class GainArmorSystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<GainArmorGA>(AddGainArmorPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<GainArmorGA>();
    }
    private IEnumerator AddGainArmorPerformer(GainArmorGA gainArmorGA)
    {
        foreach(var target in gainArmorGA.Targets)
        {
            target.GainArmor(gainArmorGA.ArmorAmount);
            yield return null; // Add VFX for adding armor
        }
    }
}
