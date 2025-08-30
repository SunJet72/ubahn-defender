using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectMultiplexFunctionType
{
    CONST,
    INCREMENT_WITH_TIPPING,
    SCALE_LINEARLY_OVER_DURATION
}

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Scriptable Objects/StatusEffect")]
public class StatusEffect : Effect
{
    // TODO: Instead of 1 parameter to effect, creat List of Characteristics (struct that has type of characteristic and value. Is used for granting stats for equip)
    //public UnitParams paramToEffect; // All speeds are effects multiplex (1.0f for 100%), all other just linear values.
    public List<UnitCharacteristics> paramsToEffect;
    public bool hasDuration;
    public float duration;
    public bool hasEffectRadius;
    public float effectRadius;
    public bool isPermanent;
    public StatusEffectMultiplexFunctionType multiplexType;

    public override EffectData EffectData => throw new System.NotImplementedException();

    public override void ApplyEffect(UnitController caster, UnitController target)
    {
        var executor = target.gameObject.AddComponent<StatusEffectExecutor>();
        executor.Init(this);
    }
}
