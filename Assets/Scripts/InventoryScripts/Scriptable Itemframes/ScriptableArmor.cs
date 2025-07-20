using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Scriptable Objects/ScriptableArmor")]

public class ScriptableArmor : ScriptableItemBase
{
<<<<<<< HEAD
    public float durability;
    public float coolness;
    public UnitData unitData;
=======
    public float additionalHealth;
    public float armor;


>>>>>>> inventory
    public void Use(GameObject player)
    {
        Debug.Log(player.name + " is equipped with " + name + " and it looks cool as " + armor);
    }
}
