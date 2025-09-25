using System;
using Fusion;
using UnityEngine;

public class SpellCaster : NetworkBehaviour
{
    private Spell spell;
    private UnitController caster;

    private bool isActiveSpell;

    private float cooldown;
    private float timeSpent;

    private int cost;
    private int curCost;
    private bool isCostEnough;

    private float ReloadPercentage { get => MathF.Min(timeSpent / cooldown, 1f); }
    private bool isReady;

    public Action<float> OnReloadPercentageChanged;
    public Action OnSpellReloaded;
    public Action<bool> OnSpellCostEnough;
    public void Init(Spell spell, UnitController caster)
    {
        isActiveSpell = false;
        this.spell = spell;
        this.caster = caster;

        if (spell is ActiveSpell activeSpell)
        {
            isActiveSpell = true;
            ActiveSpellData data = activeSpell.SpellData as ActiveSpellData;
            cooldown = data.cooldown;
            cost = data.spellCost;
            switch (data.spellCostType)
            {
                case SpellCostType.NONE:
                    cost = 0;
                    break;
                case SpellCostType.BLOOD_POINTS:
                    caster.OnBloodPointsChanged += UpdateCostValue;
                    break;
                case SpellCostType.DIRT_POINTS:
                    caster.OnDirtPointsChanged += UpdateCostValue;
                    break;
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!isReady)
        {
            timeSpent += Runner.DeltaTime;
            OnReloadPercentageChanged?.Invoke(ReloadPercentage);
            if (timeSpent >= cooldown)
            {
                isReady = true;
                timeSpent = cooldown;
                OnSpellReloaded?.Invoke();
            }
        }
    }

    private void UpdateCostValue(int value)
    {
        curCost = value;
        if (curCost >= cost != isCostEnough)
        {
            isCostEnough = curCost >= cost;
            OnSpellCostEnough?.Invoke(isCostEnough);
        }
    }

    public void CastSpell(Vector2 endPoint)
    {
        (spell as ActiveSpell).Activate(caster, endPoint);
    }

    private void ResetSpellCooldowns()
    {
        
    }

    public Spell GetSpell()
    {
        return spell;
    }
}
