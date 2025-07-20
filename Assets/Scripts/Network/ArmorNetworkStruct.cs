using Fusion;
using UnityEngine;

public struct ArmorNetworkStruct: INetworkStruct
{
    public float additionalHealth;
    public float armor;
    public float unitHealth;
    public float unitArmor;
    public float unitStrength;
    public float unitSpeed;
    public float unitAttackSpeed;
    public float unitArmorPenetration;

    public ScriptableArmor CopyData()
    {
        ScriptableArmor st = new ScriptableArmor()
        {
            unitData = new UnitData()
            {
                health = this.unitHealth,
                armor = this.unitArmor,
                strength = this.unitStrength,
                speed = this.unitSpeed,
                attackSpeed = this.unitAttackSpeed,
                armorPenetration = this.unitArmorPenetration
            },

            additionalHealth = this.additionalHealth,
            armor = this.armor,
        };
        return st;
    }
}