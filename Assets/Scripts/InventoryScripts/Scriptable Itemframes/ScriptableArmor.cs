using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Scriptable Objects/ScriptableArmor")]

public class ScriptableArmor : ScriptableItemBase
{
    public Sprite PlayerSprite;
    public Sprite PlayerBackSprite;

    //---// DEPRECATED!
    public float additionalHealth;
    public float armor;
    public UnitData unitData;
    //---//


    public void Use(GameObject player)
    {
        Debug.Log(player.name + " is equipped with " + name + " and it looks cool as " + armor);
    }
    
    public ArmorNetworkStruct CopyData()
    {
        ArmorNetworkStruct st = new ArmorNetworkStruct()
        {
            unitHealth = this.unitData.health,
            unitArmor = this.unitData.armor,
            unitStrength = this.unitData.strength,
            unitSpeed = this.unitData.speed,
            unitAttackSpeed = this.unitData.attackSpeed,
            unitArmorPenetration = this.unitData.armorPenetration,

            additionalHealth = this.additionalHealth,
            armor = this.armor,

        };
        return st;
    }
}
