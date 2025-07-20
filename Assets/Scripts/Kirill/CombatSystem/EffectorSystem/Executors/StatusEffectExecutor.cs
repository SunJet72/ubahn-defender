using System;
using System.Collections;
using Fusion;
using UnityEngine;

public class StatusEffectExecutor : NetworkBehaviour
{
    private event Action<StatusEffect> OnEffectEnd;

    private UnitController unitController;
    private StatusEffect effect;

    [Networked]
    private TickTimer effectTimer { get; set; }
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
            effectTimer = TickTimer.CreateFromSeconds(Runner, effect.duration);
            // StartCoroutine(EndEffect(effect.duration));
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsServer || !effectTimer.IsRunning || !effectTimer.Expired(Runner)) return;
        // yield return new WaitForSeconds(seconds);
        Debug.Log(OnEffectEnd);
        Debug.Log(effect);
        OnEffectEnd?.Invoke(effect);
        Runner.Despawn(Object);
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
