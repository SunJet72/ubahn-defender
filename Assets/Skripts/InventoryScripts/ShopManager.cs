using System.Collections.Generic;
using UnityEngine;

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

    public void SellExtacy()
    {
        inventory.AddItem(extacyDrink);
    }

}
