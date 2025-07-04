using UnityEngine;
using UnityEngine.Events;

public class DummyPlayerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private ScriptableWeapon weapon;
    [SerializeField] private ScriptableArmor armor;

    public UnityEvent weaponEvent;
    public UnityEvent armorEvent;
    void Start()
    {
        inventory = GetComponent<PlayerInventory>();
        shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();
        LoadInventory();
        InitGameState();
    }

    private void LoadInventory()
    {
        armor = inventory.GetCurrentArmor();
        weapon = inventory.GetCurrentWeapon();
    }

    private void InitGameState()
    {
        if (weapon.action != null)
        {
            weapon.action.SetUp(gameObject, weaponEvent);
        }
        if (armor.action != null)
        {
            armor.action.SetUp(gameObject, armorEvent);
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
}
