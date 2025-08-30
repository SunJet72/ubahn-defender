using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class UnitController : NetworkBehaviour
{
    public abstract UnitData UnitData { get; }
    public event Action<UnitController> OnDieEvent;
    public event Action<UnitController, UnitController> OnHurtEvent; // Arg1 is Attacker, Arg2 is Target
    public event Action<UnitController, UnitController> OnHurtTargetEvent; // Arg1 is Attacker, Arg2 is Target

    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    private float health { get; set; }
    [Networked]
    private float armor { get; set; }
    [Networked]
    private float strength { get; set; }
    [Networked]
    private float speed { get; set; }
    [Networked]
    private float attackSpeed { get; set; }
    [Networked]
    private float armorPenetration { get; set; }

    public float Health { get => health; }
    public float Armor { get => armor; }
    public float Strength { get => strength; }
    public float Speed { get => speed * (speedMultiplex <= 0.1 ? 0.1f : speedMultiplex); }
    public float AttackSpeed { get => attackSpeed * (attackSpeedMultiplex <= 0.1 ? 0.1f : attackSpeedMultiplex); }
    public float ArmorPenetration { get => armorPenetration >= 90f ? 90f : armorPenetration; }

    private float speedMultiplex;
    private float attackSpeedMultiplex;

    private List<StatusEffect> curStatusEffects;
    protected void Init()
    {
        health = 0;
        armor = 0;
        strength = 0;
        speed = 0;
        attackSpeed = 0;
        armorPenetration = 0;

        speedMultiplex = 1f;
        attackSpeedMultiplex = 1f;

        curStatusEffects = new List<StatusEffect>();

        ApplyUnitDataStats(UnitData);
    }

    protected void ApplyUnitDataStats(UnitData unitData)
    {
        Debug.Log("attack speed in giver UnitData: " + unitData.attackSpeed);
        Debug.Log("attack speed multiplex: " + speedMultiplex);
        Debug.Log("attack speed just: " + attackSpeed);
        Debug.Log("attack speed calculated: " + AttackSpeed);

        health += unitData.health;
        armor += unitData.armor;
        strength += unitData.strength;
        speed += unitData.speed;
        attackSpeed += unitData.attackSpeed;
        armorPenetration += unitData.armorPenetration;

        Debug.Log("Attack Speed after adding just: " + attackSpeed);
        Debug.Log("Current attack speed:" + AttackSpeed);
    }

    public void ApplyStatusEffect(StatusEffect statusEffect, float multiplex)
    {
        curStatusEffects.Add(statusEffect);
        CalculateStatusEffect(statusEffect, multiplex);
    }

    public void UpdateStatusEffect(StatusEffect statusEffect, float prevMultiplex, float multiplex)
    {
        CalculateStatusEffect(statusEffect, -prevMultiplex);
        CalculateStatusEffect(statusEffect, multiplex);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect, float multiplex)
    {
        Debug.Log("I remove effect: " + statusEffect);
        CalculateStatusEffect(statusEffect, -multiplex);
        curStatusEffects.Remove(statusEffect);
    }

    private void CalculateStatusEffect(StatusEffect statusEffect, float multiplex)
    {
        if (statusEffect.paramsToEffect == null)
            return;
        foreach (var characteristics in statusEffect.paramsToEffect)
        {
            switch (characteristics.param)
            {
                case UnitParams.HEALTH:
                    health += characteristics.value * multiplex;
                    if (health <= 0)
                    {
                        health = 0;
                        Die();
                    }
                    break;
                case UnitParams.ARMOR:
                    armor += characteristics.value * multiplex;
                    break;
                case UnitParams.ARMOR_PENETRATION:
                    armorPenetration += characteristics.value * multiplex;
                    break;
                case UnitParams.STRENGTH:
                    strength += characteristics.value * multiplex;
                    break;
                case UnitParams.SPEED:
                    speedMultiplex += characteristics.value * multiplex;
                    break;
                case UnitParams.ATTACK_SPEED:
                    attackSpeedMultiplex += characteristics.value * multiplex;
                    break;
            }
        } 
    }

    protected virtual void Hurt(float damage, float penetration, UnitController attacker)
    {
        Debug.Log("Unit was hurt " + gameObject + " Damage: " + damage + " penetration: " + penetration);
        Debug.Log("Health before: " + Health);
        health -= damage * (100f / (100f + this.armor - penetration));
        Debug.Log("Health after: " + Health);
        OnHurtEvent?.Invoke(attacker, this);
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    public void Hit(UnitController target, float damage)
    {
        OnHurtTargetEvent?.Invoke(this, target);
        target.Hurt(CalculateDamage(damage), ArmorPenetration, this);
    }
    private float CalculateDamage(float damage)
    {
        return damage * ((100f + Strength) / 100f);
    }


    public virtual void OnHealthChanged()
    {
        Debug.Log("OnHealthChanged has no override, but it's ok");
    }

    protected abstract void Die(); // In all Fällen OnDieEvent shooten

    protected void TriggerDeathEvent()
    {
        OnDieEvent?.Invoke(this);
    }
}
