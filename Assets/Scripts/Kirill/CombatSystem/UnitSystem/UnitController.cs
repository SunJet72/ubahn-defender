using System;
using Fusion;
using UnityEngine;

public abstract class UnitController : NetworkBehaviour
{
    protected abstract UnitData UnitData { get; }

    [Networked]
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
    public float Speed { get => speed * (speedMultiplex <= 0.1 ? 0.1f: speedMultiplex); }
    public float AttackSpeed { get => attackSpeed * (attackSpeedMultiplex <= 0.1 ? 0.1f: attackSpeedMultiplex); }
    public float ArmorPenetration { get => armorPenetration; }

    private float speedMultiplex;
    private float attackSpeedMultiplex;
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

        ApplyUnitDataStats(UnitData);
    }

    protected void ApplyUnitDataStats(UnitData unitData)
    {
        health += UnitData.health;
        armor += UnitData.armor;
        strength += UnitData.strength;
        speed += UnitData.speed;
        attackSpeed += UnitData.attackSpeed;
        armorPenetration += UnitData.armorPenetration;
    }

    public void ApplyStatusEffect(StatusEffect statusEffect, Action<StatusEffect> onEndAction, bool isPermanent)
    {
        // Applying effect
        switch (statusEffect.paramToEffect)
        {
            case UnitParams.HEALTH:
                health += statusEffect.value;
                if (health <= 0)
                {
                    health = 0;
                    Die();
                }
                break;
            case UnitParams.ARMOR:
                armor += statusEffect.value;
                break;
            case UnitParams.ARMOR_PENETRATION:
                armorPenetration += statusEffect.value;
                break;
            case UnitParams.STRENGTH:
                strength += statusEffect.value;
                break;
            case UnitParams.SPEED:
                speedMultiplex += statusEffect.value;
                break;
            case UnitParams.ATTACK_SPEED:
                attackSpeedMultiplex += statusEffect.value;
                break;
        }
        // Subscribe on event in effector to then cleanse the effect
        if (!isPermanent)
            onEndAction += RemoveStatusEffect;
    }

    private void RemoveStatusEffect(StatusEffect statusEffect)
    {
        // Reverting application of an effect
        switch (statusEffect.paramToEffect)
        {
            case UnitParams.HEALTH:
                health -= statusEffect.value;
                if (health <= 0)
                {
                    health = 0;
                    Die();
                }
                break;
            case UnitParams.ARMOR:
                armor -= statusEffect.value;
                break;
            case UnitParams.ARMOR_PENETRATION:
                armorPenetration -= statusEffect.value;
                break;
            case UnitParams.STRENGTH:
                strength -= statusEffect.value;
                break;
            case UnitParams.SPEED:
                speedMultiplex -= statusEffect.value;
                break;
            case UnitParams.ATTACK_SPEED:
                attackSpeedMultiplex -= statusEffect.value;
                break;
        }
    }

    public void Hurt(float damage, float enemyPenetration)
    {
        health -= damage * (100f / (100f + this.armor - enemyPenetration));
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    protected abstract void Die();
}
