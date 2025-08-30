using UnityEngine;

public class DamageEffect : Effect
{
    [SerializeField] private DamageEffectData data;
    public override EffectData EffectData => data;

    public override void ApplyEffect(UnitController caster, UnitController target)
    {
        caster.Hit(target, data.damage);
    }
}
