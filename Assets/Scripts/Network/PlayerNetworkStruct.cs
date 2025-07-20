using Fusion;

public struct PlayerNetworkStruct : INetworkStruct
{
    public float unitHealth;
    public float unitArmor;
    public float unitStrength;
    public float unitSpeed;
    public float unitAttackSpeed;
    public float unitArmorPenetration;
    public PlayerClass playerClass;

    public PlayerCombatSystemData CopyData()
    {
        PlayerCombatSystemData st = new PlayerCombatSystemData()
        {
            health = this.unitHealth,
            armor = this.unitArmor,
            strength = this.unitStrength,
            speed = this.unitSpeed,
            attackSpeed = this.unitAttackSpeed,
            armorPenetration = this.unitArmorPenetration,
          
            playerClass = this.playerClass
        };
        return st;
    }
}