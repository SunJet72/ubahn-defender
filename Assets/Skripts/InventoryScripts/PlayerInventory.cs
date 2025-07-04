using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public UnityEvent InventoryChanged;
    private GameObject player;

    [SerializeField] private List<InventorySlot> inventoryStash = new List<InventorySlot>();
    [SerializeField] private ScriptableArmor currentArmor;
    [SerializeField] private ScriptableWeapon currentWeapon;
    [SerializeField] private List<ScriptableBuff> Consumables = new List<ScriptableBuff>();

    void Start()
    {
        player = transform.gameObject;
        if (currentArmor == null)
        {
            currentArmor = ItemManager.instance.emptyArmor;
        }
        if (currentWeapon == null)
        {
            currentWeapon = ItemManager.instance.emptyWeapon;
        }

        InventoryChanged.Invoke();
    }

    public PlayerInventory AddItem(ScriptableItemBase item)
    {
        foreach (InventorySlot slot in inventoryStash)
        {
            if (slot.Sample.name == item.name && slot.Sample.maxStackSize > slot.Count)
            {
                slot.AddItem(item);
                InventoryChanged.Invoke();
                Debug.Log(slot.Sample.maxStackSize);
                return this;
            }
        }
        inventoryStash.Add(new InventorySlot(item).AddItem(item));
        InventoryChanged.Invoke();
        Debug.Log(typeof(PlayerInventory));
        return this;
    }

    public PlayerInventory RemoveItem(ScriptableItemBase item)
    {
        foreach (InventorySlot slot in inventoryStash)
        {
            if (slot.Sample.name == item.name)
            {
                slot.RemoveItem();
                if (slot.Count == 0)
                {
                    inventoryStash.Remove(slot);
                }
                InventoryChanged.Invoke();
                return this;
            }
        }
        Debug.LogError("Removing nonexisitng item");

        return this;
    }

    public bool Contains(ScriptableItemBase item)
    {
        foreach (InventorySlot slot in inventoryStash)
        {
            if (slot.Sample.name == item.name)
            {
                return true;
            }
        }
        return false;
    }

    public void SwapOutCurrentArmor(ScriptableItemBase armor)
    {
        if (currentArmor is not ScriptableArmor)
        {
            Debug.Log("Bro is trying to wear not currentArmor and dies from cringe");
            return;
        }
        if (currentArmor != ItemManager.instance.emptyArmor)
        {
            AddItem(currentArmor);
        }
        RemoveItem(armor);
        currentArmor = (ScriptableArmor)armor;
    }

    public void SwapOutCurrentWeapon(ScriptableItemBase weapon)
    {
        if (weapon is not ScriptableWeapon)
        {
            Debug.Log("Bro is trying to equip not a weapon and dies from cringe");
            return;
        }
        if (currentWeapon != ItemManager.instance.emptyWeapon)
        {
            AddItem(currentWeapon);
        }
        RemoveItem(weapon);
        currentWeapon = (ScriptableWeapon)weapon;
    }

    public List<InventorySlot> GetInventory()
    {
        return inventoryStash;
    }
    public override string ToString()
    {
        StringBuilder bld = new StringBuilder();
        bld.Append("Player has \n");
        foreach (InventorySlot slot in inventoryStash)
        {
            bld.Append(slot).Append("\n");
        }
        return bld.ToString();
    }


    public ScriptableArmor GetCurrentArmor()
    {
        return currentArmor;
    }

    public ScriptableWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
