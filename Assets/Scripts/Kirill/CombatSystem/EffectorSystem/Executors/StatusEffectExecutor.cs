using System;
using System.Collections;
using Fusion;
using UnityEngine;

public class StatusEffectExecutor : NetworkBehaviour
{
    private event Action<StatusEffect, float, float> OnEffectUpdate;

    private UnitController unitController;
    private StatusEffect effect;
    private float multiplex;

    [Networked]
    private TickTimer effectTimer { get; set; }

    public override void Spawned()
    {
        if (effect.hasDuration)
        {
            effectTimer = TickTimer.CreateFromSeconds(Runner, effect.duration);
            // StartCoroutine(EndEffect(effect.duration));
        }
    }
    public void Init(StatusEffect effect)
    {
        this.effect = effect;
        switch (effect.multiplexType)
        {
            case StatusEffectMultiplexFunctionType.CONST:
                multiplex = 1f;
                break;
            case StatusEffectMultiplexFunctionType.INCREMENT_WITH_TIPPING:
                multiplex = 1f;
                break;
            case StatusEffectMultiplexFunctionType.SCALE_LINEARLY_OVER_DURATION:
                multiplex = 0f; // Potentially with start value, but I dont use this for now;
                break;
        }

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
                unitController.ApplyStatusEffect(effect, multiplex);
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (effect.multiplexType == StatusEffectMultiplexFunctionType.SCALE_LINEARLY_OVER_DURATION)
        {
            if (!effect.hasDuration) return;
            multiplex += Runner.DeltaTime / effect.duration;

        }

        if (!Runner.IsServer || !effectTimer.IsRunning || !effectTimer.Expired(Runner)) return;
        // yield return new WaitForSeconds(seconds);
        Debug.Log(effect);
        unitController.RemoveStatusEffect(effect, multiplex);
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
                unit.ApplyStatusEffect(effect, multiplex);
            }
        }
    }

    public void Tip()
    {
        if (effect.multiplexType == StatusEffectMultiplexFunctionType.INCREMENT_WITH_TIPPING)
        {
            multiplex++;
            OnEffectUpdate?.Invoke(effect, multiplex - 1, multiplex);
        }
    }
}
