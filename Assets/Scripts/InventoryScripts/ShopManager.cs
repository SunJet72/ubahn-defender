using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ShopManager : MonoBehaviour
{
    private GameObject player;
    private PlayerInventory inventory;

    public ScriptableItemBase extacyDrink;

    [SerializeField]private List<ScriptableItemBase> stock;
    void Start()
    {
        player = GameObject.Find("Player");
        inventory = player.GetComponent<PlayerInventory>();
        stock = ItemManager.instance.GetAll();
    }

    public void SellRandomItem()
    {
        inventory.AddItem(stock?[Random.Range(0, stock.Count)]);
    }

    public void SellRandomArmor()
    {
        List<ScriptableItemBase> armor = (stock ?? new List<ScriptableItemBase>()).Where(a => a != null && a is ScriptableArmor).ToList();
        inventory.AddItem(armor?[Random.Range(0, armor.Count)]);
    }
    public void SellRandomWeapon()
    {
        List<ScriptableItemBase> weapons = (stock ?? new List<ScriptableItemBase>()).Where(a => a != null && a is ScriptableWeapon).ToList();
        inventory.AddItem(weapons?[Random.Range(0, weapons.Count)]);
    }

      public void SellRandomConsumable()
    {
        List<ScriptableItemBase> consumables = (stock ?? new List<ScriptableItemBase>()).Where(a => a != null && a is ScriptableConsumable).ToList();
        inventory.AddItem(consumables?[Random.Range(0, consumables.Count)]);
    }

    public void SellExtacy()
    {
        inventory.AddItem(extacyDrink);
    }

}
