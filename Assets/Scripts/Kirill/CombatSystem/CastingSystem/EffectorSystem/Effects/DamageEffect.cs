using UnityEngine;

public class DamageEffect : InstantEffectExecutor
{
    [SerializeField] private float damage;
    protected override void ApplyInstantEffect(StatefulExecutor executor)
    {
        executor.GetCaster().Hit(executor.GetTarget(), damage);
    }
}
