using Fusion;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Scriptable Objects/ScriptableWeapon")]

public class ScriptableWeapon : ScriptableItemBase
{
    public float damage;
    public float attackSpeed;
    public float armorPenetration;
    public float range;
    public float cooldown;


    public UnitData unitData;
    public bool canShoot;
    public NetworkObject projectile;
    public bool canDealSplashDamage;
    public float fov;


    public void Use(GameObject player)
    {
        Debug.Log(player.name + "attacks with " + name + " and deals " + damage + " damage");
    }

    public WeaponNetworkStruct CopyData()
    {
        WeaponNetworkStruct st = new WeaponNetworkStruct()
        {
            unitHealth = this.unitData.health,
            unitArmor = this.unitData.armor,
            unitStrength = this.unitData.strength,
            unitSpeed = this.unitData.speed,
            unitAttackSpeed = this.unitData.attackSpeed,
            unitArmorPenetration = this.unitData.armorPenetration,

            damage = this.damage,
            attackSpeed = this.attackSpeed,
            armorPenetration = this.armorPenetration,
            range = this.range,
            cooldown = this.cooldown,

            canShoot = this.canShoot,
            canDealSplashDamage = this.canDealSplashDamage,

            fov = this.fov,
            projectileName = projectile
        };
        return st;
    }
}
