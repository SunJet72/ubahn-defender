using UnityEngine;

public class UIInventoryController : MonoBehaviour
{

    [SerializeField] private GameObject player;
    private PlayerInventory inventory;

    [SerializeField] private UIInventoryStash stash;
    [SerializeField] private UIItemSlot UIarmor;
    [SerializeField] private UIItemSlot UIweapon;

    private UIInventorySlector selector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        inventory = player.GetComponent<PlayerInventory>();
        inventory.InventoryChanged.AddListener(Rebuild);
        selector = GetComponent<UIInventorySlector>();
    }

    public void Rebuild()
    {
        switch (selector.state)
        {
            case UIInventorySlector.SelectorState.SelectingArmor:
                //stash.ShowArmorOptions(inventory.GetInventory());
                stash.Rebuild(inventory.GetInventory());
                break;
            case UIInventorySlector.SelectorState.SelectingWeapon:
                //stash.ShowWeaponOptions(inventory.GetInventory());
                stash.Rebuild(inventory.GetInventory());
                break;
            default:
                stash.Rebuild(inventory.GetInventory());
                break;
        }
        UIarmor.RefreshSlot(inventory.GetCurrentArmor());
        UIweapon.RefreshSlot(inventory.GetCurrentWeapon());
    }

}
