using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DummyPlayerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PlayerInventory inventory;
    [SerializeField] private ScriptableWeapon weapon;
    [SerializeField] private ScriptableArmor armor;
    [SerializeField] public InventorySlot[] activeConsumables;
    [SerializeField]private int _activeConsumableIndex = 0;
    public int ActiveConsumableIndex
    {
        get => _activeConsumableIndex;
        set
        {
            if (value < 0)
            {
                _activeConsumableIndex = 0;
            }
            else if (value > inventory.GetMaxActiveConsumables() - 1)
            {
                _activeConsumableIndex = inventory.GetMaxActiveConsumables() - 1;
            }
            else
            {
                _activeConsumableIndex = value;
            }
        }
    }
    Component weaponComponent;
    Component armorComponent;
    Component[] consumableComponents;


    [HideInInspector] public UnityEvent weaponEvent;
    [HideInInspector] public UnityEvent armorEvent;
     public UnityEvent[] consumableEvents;

    void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        inventory.EquipmentChanged.AddListener(LoadInventory);
        inventory.EquipmentChanged.AddListener(InitPlayerActions);
    }
    void Start()
    {
        // inventory = GetComponent<PlayerInventory>();
        // shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();
        // LoadInventory();
        // InitGameState();
    }

    private void LoadInventory()
    {
        armor = inventory.GetCurrentArmor();
        weapon = inventory.GetCurrentWeapon();
        activeConsumables = inventory.GetActiveConsumables();
        if (consumableComponents == null || consumableEvents == null)
        {
            consumableEvents = new UnityEvent[inventory.GetMaxActiveConsumables()];
            consumableComponents = new Component[inventory.GetMaxActiveConsumables()];
            for (int i = 0; i < consumableEvents.Length; ++i)
            {
                consumableEvents[i] = new UnityEvent();
            }
        }
        UIMasterController.instance.RebuildConsumableSelector();
    }

    private void InitPlayerActions()
    {
        //Debug.Log("Initing statae");
        if (weaponComponent != null)
        {
            Destroy(weaponComponent);
        }
        if (armorComponent != null)
        {
            Destroy(armorComponent);
        }
        for(int i =0; i<consumableComponents.Length; ++i)
        {
            Destroy(consumableComponents[i]);
        }

        for (int i = 0; i < activeConsumables.Length; ++i)
        {
            if (activeConsumables[i].GetSample() != ItemManager.instance.emptyItem&&activeConsumables[i].GetSample().action!=null)
            {
                consumableComponents[i] = activeConsumables[i].GetSample().action.SetUp(gameObject, consumableEvents[i]);
            }
        }
        if (weapon.action != null)
        {
            weaponComponent = weapon.action.SetUp(gameObject, weaponEvent);
        }
        if (armor.action != null)
        {
            armorComponent = armor.action.SetUp(gameObject, armorEvent);
        }
    }

    public void InvokeWeaponAction()
    {
        weaponEvent.Invoke();
    }

    public void InvokeArmorAction()
    {
        armorEvent.Invoke();
    }

    public void InvokeCosumableAction()
    {
        if (activeConsumables[ActiveConsumableIndex].Count > 0)
        {
            consumableEvents[ActiveConsumableIndex].Invoke();
            activeConsumables[ActiveConsumableIndex].RemoveItem();
            //UIMasterController.instance.RebuildConsumableSelector();
            UIMasterController.instance.RebuildAll();
        }
    }

}
