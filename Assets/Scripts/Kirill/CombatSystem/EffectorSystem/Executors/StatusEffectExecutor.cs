using System;
using System.Collections;
using UnityEngine;

public class StatusEffectExecutor : MonoBehaviour
{
    private event Action<StatusEffect> OnEffectEnd;

    private UnitController unitController;
    private StatusEffect effect;
    public void Init(StatusEffect effect)
    {
        this.effect = effect;
        transform.TryGetComponent<UnitController>(out unitController);
        if (unitController != null) // it stil can be null, if we want to apply e.g. on a random point with area
        {
            unitController.ApplyStatusEffect(effect, OnEffectEnd);
        }
        if (effect.hasDuration)
        {
            StartCoroutine(EndEffect(effect.duration));
        }
        if (effect.hasEffectRadius)
        {
            // TODO: Apply Effect on all Units inside the radius
        }
    }

    private IEnumerator EndEffect(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnEffectEnd.Invoke(effect);
        Destroy(this);
    }
}
