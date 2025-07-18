using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    private GameObject player;
    private PlayerInventory inventory;

    private Station currentStation;

    [SerializeField] private List<ScriptableItemBase> stock;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        inventory = PlayerInventory.instance;
        currentStation = WorldMapController.instance.currentStation;
        InitShopForStation(currentStation);
        //UIMasterController.instance.RebuildShop();
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

    public void InitShopForStation(Station station)
    {
        currentStation = station;
        stock = ItemManager.instance.GetAll(0);
        for (int i = 1; i <= currentStation.StationTier; ++i)
        {
            stock.AddRange(ItemManager.instance.GetAll(i));
        }
        UIMasterController.instance.RebuildShop(currentStation);
    }

    public void SellItem(ScriptableItemBase item)
    {
        if (inventory.MoneySpend(item.price))
        {
            inventory.AddItem(item);
        }
        else
        {
            Debug.Log("No dope for you lil shit");
        }
    }


    public List<ScriptableItemBase> GetStock()
    {
        return stock;
    }

    public int GetTier()
    {
        return currentStation.StationTier;
    }


}
