using Fusion;

public struct WeaponNetworkStruct: INetworkStruct
{
    public float damage;
    public float attackSpeed;
    public float armorPenetration;
    public float range;
    public float cooldown;
    public float unitHealth;
    public float unitArmor;
    public float unitStrength;
    public float unitSpeed;
    public float unitAttackSpeed;
    public float unitArmorPenetration;
    public bool canShoot;
    public NetworkObject projectileName;
    public bool canDealSplashDamage;
    public float fov;

    public ScriptableWeapon CopyData()
    {
        ScriptableWeapon st = new ScriptableWeapon()
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

            damage = this.damage,
            attackSpeed = this.attackSpeed,
            armorPenetration = this.armorPenetration,
            range = this.range,
            cooldown = this.cooldown,

            canShoot = this.canShoot,
            canDealSplashDamage = this.canDealSplashDamage,

            fov = this.fov,
            projectile = this.projectileName
        };
        return st;
    }
}