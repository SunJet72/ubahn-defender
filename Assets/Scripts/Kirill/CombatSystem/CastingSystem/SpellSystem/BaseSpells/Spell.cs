using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class Spell : ScriptableObject
{
    public abstract SpellData SpellData { get; }

    protected void ApplyEffect(UnitController caster, UnitController target, Effect effect)
    {
        effect.ApplyEffect(caster, target);
    }

    private void ExecuteOnPassiveEffects()
    {
        foreach (Effect effect in SpellData.OnPassiveEffects)
            ApplyEffect(null, null, effect); //TODO: Find owner;
    }
}
