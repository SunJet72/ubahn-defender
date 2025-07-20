using UnityEngine;
public enum PlayerClass
{
    WARRIOR,
    RANGER,
    INGENIEUR
}
[CreateAssetMenu(fileName = "PlayerCombatSystemData", menuName = "Scriptable Objects/PlayerCombatSystemData")]
public class PlayerCombatSystemData : UnitData
{
    public PlayerClass playerClass;

    public PlayerNetworkStruct CopyData()
    {
        PlayerNetworkStruct st = new PlayerNetworkStruct()
        {
            unitHealth = this.health,
            unitArmor = this.armor,
            unitStrength = this.strength,
            unitSpeed = this.speed,
            unitAttackSpeed = this.attackSpeed,
            unitArmorPenetration = this.armorPenetration,
            playerClass = this.playerClass
        };
        return st;
    }
}
