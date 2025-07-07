using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class DummyPlayerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private ScriptableWeapon weapon;
    [SerializeField] private ScriptableArmor armor;
    [SerializeField]private Component weaponComponent;
    [SerializeField]private Component armorComponent;

    public UnityEvent weaponEvent;
    public UnityEvent armorEvent;

    void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();
        LoadInventory();
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
}
