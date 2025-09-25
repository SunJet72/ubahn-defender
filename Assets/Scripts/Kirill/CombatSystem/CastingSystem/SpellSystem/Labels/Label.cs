using UnityEngine;

public abstract class Label : MonoBehaviour
{
    protected enum Status
    {
        ARMOR,
        WEAPON,
        NONE
    }
    protected Status status = Status.NONE;

    protected PlayerCombatSystem player;
    protected WeaponGO weapon;
    protected ArmorGO armor;
    public void Init(PlayerCombatSystem player, WeaponGO weapon)
    {
        this.player = player;
        this.weapon = weapon;
        status = Status.WEAPON;
    }

    public void Init(PlayerCombatSystem player, ArmorGO armor)
    {
        this.player = player;
        this.armor = armor;
        status = Status.ARMOR;
    }

    protected abstract void InitAsWeapon();
    protected abstract void InitAsArmor();
}
