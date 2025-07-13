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

    void Awake()
    {
        health = UnitData.health;
        armor = UnitData.armor;
        strength = UnitData.strength;
        speed = UnitData.speed;
        attackSpeed = UnitData.attackSpeed;
        armorPenetration = UnitData.armorPenetration;

        speedMultiplex = 1f;
        attackSpeedMultiplex = 1f;
    }

    public void ApplyEffect()
    {
        // Subscribe on event in effector to then cleanse the effect
    }

    private void RemoveEffect()
    {
        
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
