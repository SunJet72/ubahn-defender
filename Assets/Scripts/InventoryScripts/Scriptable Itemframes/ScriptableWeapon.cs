using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Scriptable Objects/ScriptableWeapon")]

public class ScriptableWeapon : ScriptableItemBase
{
    public float damage;
    public float cooldown;
    public float range;

    public UnitData unitData;
    public GameObject spell;


    public void Use(GameObject player)
    {
        Debug.Log(player.name + "attacks with " + name + " and deals " + damage + " damage");
    }
}
