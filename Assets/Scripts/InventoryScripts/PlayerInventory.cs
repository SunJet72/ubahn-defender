using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public UnityEvent InventoryChanged;
    public UnityEvent EquipmentChanged;

    [SerializeField] private List<InventorySlot> inventoryStash = new List<InventorySlot>();
    [SerializeField] private ScriptableArmor currentArmor;
    [SerializeField] private ScriptableWeapon currentWeapon;
    [SerializeField] private int maxActiveConsumables;
    [SerializeField] private InventorySlot[] activeConsumables;


    void Awake()
    {
        if (maxActiveConsumables == 0)
        {
            maxActiveConsumables = 1;
        }
        if (activeConsumables == null)
        {
            activeConsumables = new InventorySlot[maxActiveConsumables];
        }
        else
        {
            maxActiveConsumables = activeConsumables.Length;
        }
    }

    void Start()
    {
        for(int i =0; i< maxActiveConsumables;++i){
            if (activeConsumables[i] == null)
            {
                Debug.Log("Filling emptiness");
                activeConsumables[i] = new InventorySlot(ItemManager.instance.emptyItem);
            }
        }
        if (currentArmor == null)
        {
            currentArmor = ItemManager.instance.emptyArmor;
        }
        if (currentWeapon == null)
        {
            currentWeapon = ItemManager.instance.emptyWeapon;
        }

        InventoryChanged.Invoke();
        EquipmentChanged.Invoke();
    }

    public PlayerInventory AddItem(ScriptableItemBase item)
    {
        foreach (InventorySlot slot in inventoryStash)
        {
            if (slot.GetSample().name == item.name && slot.GetSample().maxStackSize > slot.Count)
            {
                slot.AddItem(item);
                InventoryChanged.Invoke();
                Debug.Log(slot.GetSample().maxStackSize);
                return this;
            }
        }
        inventoryStash.Add(new InventorySlot(item).AddItem(item));
        InventoryChanged.Invoke();
        return this;
    }

    public PlayerInventory RemoveItem(ScriptableItemBase item)
    {
        foreach (InventorySlot slot in inventoryStash)
        {
            if (slot.GetSample().name == item.name)
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

    public PlayerInventory RemoveSlot(InventorySlot slot)
    {
        inventoryStash.Remove(slot);
        return this;
    }

    public bool Contains(ScriptableItemBase item)
    {
        foreach (InventorySlot slot in inventoryStash)
        {
            if (slot.GetSample().name == item.name)
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
        InventoryChanged.Invoke();
        EquipmentChanged.Invoke();
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
        InventoryChanged.Invoke();
        EquipmentChanged.Invoke();
    }

    public void AddToActiveCosumables(InventorySlot slot, int slotIndex)
    {
        Debug.Log("trying to put smth in " + slotIndex);
        if (slot.GetSample() is not ScriptableConsumable)
        {
            Debug.Log("Bro is trying to equip not a consumable and dies from cringe");
            return;
        }
        if (activeConsumables[slotIndex].GetSample() != ItemManager.instance.emptyItem&& activeConsumables[slotIndex].Count!=0)
        {
            inventoryStash.Add(activeConsumables[slotIndex]);
        }
        RemoveSlot(slot);
        activeConsumables[slotIndex] = slot;
        InventoryChanged.Invoke();
        EquipmentChanged.Invoke();
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

    public InventorySlot[] GetActiveConsumables()
    {
        return activeConsumables;
    }

    public int GetMaxActiveConsumables()
    {
        return maxActiveConsumables;
    }
}
