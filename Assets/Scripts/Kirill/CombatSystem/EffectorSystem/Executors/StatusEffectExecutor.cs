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
        if (effect.hasEffectRadius)
        {
            ApplyWithingRadius();
        }
        else
        {
            transform.TryGetComponent<UnitController>(out unitController);
            if (unitController != null) // it stil can be null, if we want to apply e.g. on a random point with area
            {
                unitController.ApplyStatusEffect(effect, OnEffectEnd, effect.isPermanent);
            }
        }
        if (effect.hasDuration)
        {
            StartCoroutine(EndEffect(effect.duration));
        }
    }

    private IEnumerator EndEffect(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnEffectEnd.Invoke(effect);
        Destroy(this);
    }

    private void ApplyWithingRadius()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, effect.effectRadius); // Potentially adding LayerMask

        foreach (Collider2D col in hits)
        {
            UnitController unit = col.GetComponent<UnitController>();
            if (unit != null) // Apply effect only on enemies
            {
                unit.ApplyStatusEffect(effect, OnEffectEnd, effect.isPermanent);
            }
        }
    }
}
