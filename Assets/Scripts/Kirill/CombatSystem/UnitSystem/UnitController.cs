using System;
using UnityEngine;

public abstract class UnitController : MonoBehaviour
{
    protected abstract UnitData UnitData { get; }

    private float health;
    private float armor;
    private float strength;
    private float speed;
    private float attackSpeed;
    private float armorPenetration;

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

    public void ApplyStatusEffect(StatusEffect statusEffect)
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
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        Debug.Log("I remove effect: " + statusEffect);
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
