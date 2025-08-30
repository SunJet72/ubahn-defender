using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class Spell : NetworkBehaviour
{
    public abstract SpellData SpellData { get; }

    protected void TryExecuteEffect(UnitController caster, UnitController target, Effect effect)
    {
        bool isFit = false;
        foreach (EffectTargetFilter filter in effect.EffectData.filters)
        {
            switch (filter)
            {
                case EffectTargetFilter.SELF:
                    if (caster == target) // Does == work normally?
                        isFit = true;
                    break;
                case EffectTargetFilter.ENEMIES:
                    if (target.UnitData.unitType == UnitType.ENEMY)
                        isFit = true;
                    break;
                case EffectTargetFilter.VEHICLES:
                    if (target.UnitData.unitType == UnitType.VEHICLE)
                        isFit = true;
                    break;
                case EffectTargetFilter.ALLIES:
                    if (target.UnitData.unitType == UnitType.PLAYER && caster != target)
                        isFit = true;
                    break;
            }
        }
        if (isFit)
        {
            // Cast effect on a target
            effect.ApplyEffect(caster, target);
        }
    }

    private void ExecuteOnPassiveEffects()
    {
        foreach (Effect effect in SpellData.OnPassiveEffects)
            TryExecuteEffect(null, null, effect); //TODO: Find owner;
    }
}
