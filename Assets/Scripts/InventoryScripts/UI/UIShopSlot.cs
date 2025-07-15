using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopSlot : MonoBehaviour
{
    [SerializeField] public int itemId;// { get; private set; }
    [SerializeField] private Image itemSprite;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Color commonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color legendaryColor;

    public void Rebuild(int id)
    {
        itemId = id;
        ScriptableItemBase item = ItemManager.instance.getItem(id);
        if (item == null)
        {
            Debug.LogError("ShopSlot could not fetch requested item");
            item = ItemManager.instance.emptyItem;
        }
        itemSprite.sprite = item.sprite;
        titleText.text = item.title;
        StringBuilder str = new StringBuilder();
        priceText.text = str.Append(item.price).Append(" $").ToString();

        buyButton.onClick.AddListener(() => ShopManager.instance.SellItem(item));
        switch (item.tier)
        {
            case 0:
                GetComponent<Image>().color = commonColor;
                break;
            case 1:
                GetComponent<Image>().color = rareColor;
                break;
            case 2:
                GetComponent<Image>().color = legendaryColor;
                break;

            default:
                GetComponent<Image>().color = commonColor;
                break;
        }

    } 
    public void Rebuild()
    {
        ScriptableItemBase item = ItemManager.instance.getItem(itemId);
        if (item == null)
        {
            Debug.LogError("ShopSlot could not fetch requested item");
            item = ItemManager.instance.emptyItem;
        }
        itemSprite.sprite = item.sprite;
        titleText.text = item.title;
        StringBuilder str = new StringBuilder();
        priceText.text = str.Append(item.price).Append(" $").ToString();

        buyButton.onClick.AddListener(() => ShopManager.instance.SellItem(item));
        switch (item.tier)
        {
            case 0:
                GetComponent<Image>().color = commonColor;
                break;
            case 1:
                GetComponent<Image>().color = rareColor;
                break;
            case 2:
                GetComponent<Image>().color = legendaryColor;
                break;

            default:
                GetComponent<Image>().color = commonColor;
                break;
        }
    } 
}
