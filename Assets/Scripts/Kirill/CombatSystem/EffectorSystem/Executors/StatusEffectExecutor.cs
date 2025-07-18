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
            ApplyWithinRadius();
        }
        else
        {
            transform.TryGetComponent<UnitController>(out unitController);
            if (unitController != null) // it stil can be null, if we want to apply e.g. on a random point with area
            {
                Debug.Log("I apply status effect: " + effect);
                unitController.ApplyStatusEffect(effect);
                if (!effect.isPermanent)
                    OnEffectEnd += unitController.RemoveStatusEffect;
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
        Debug.Log(OnEffectEnd);
        Debug.Log(effect);
        OnEffectEnd?.Invoke(effect);
        Destroy(this);
    }

    private void ApplyWithinRadius()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, effect.effectRadius); // Potentially adding LayerMask

        foreach (Collider2D col in hits)
        {
            UnitController unit = col.GetComponent<UnitController>();
            if (unit != null) // Apply effect only on enemies
            {
                unit.ApplyStatusEffect(effect);
                if (!effect.isPermanent)
                    OnEffectEnd += unitController.RemoveStatusEffect;
            }
        }
    }
}
