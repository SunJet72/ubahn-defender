using System.Text;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItemTier : MonoBehaviour
{
    [SerializeField] int tier;
    [SerializeField] TMP_Text tierText;
    [SerializeField] private GameObject UISlotPrefab;
    [SerializeField] private GameObject content;

    public void Rebuild(List<ScriptableItemBase> items, int tier)
    {
        this.tier = tier;
        StringBuilder str = new StringBuilder();
        tierText.text = str.Append("Tier ").Append(tier).Append(" items:").ToString();

        for (int diff = content.transform.childCount - 1; diff >= items.Count; --diff)
        {
            Destroy(content.transform.GetChild(diff).gameObject);
        }

        for (int diff = content.transform.childCount; diff < items.Count; diff++)
        {
            GameObject obj = Instantiate(UISlotPrefab, content.transform);
        }

        for (int i = 0; i < items.Count; ++i)
        {
            UIShopSlot UIslot = content.transform.GetChild(i).gameObject.GetComponent<UIShopSlot>();
            UIslot.Rebuild(items[i].id);
        }
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

    }

    public void Rebuild()
    {
        List<ScriptableItemBase> items = ItemManager.instance.GetAll();
        StringBuilder str = new StringBuilder();
        tierText.text = str.Append("Tier ").Append(tier).Append(" items:").ToString();

        for (int diff = content.transform.childCount - 1; diff >= items.Count; --diff)
        {
            Destroy(content.transform.GetChild(diff).gameObject);
        }

        for (int diff = content.transform.childCount; diff < items.Count; diff++)
        {
            GameObject obj = Instantiate(UISlotPrefab, content.transform);
        }

        for (int i = 0; i < items.Count; ++i)
        {
            UIShopSlot UIslot = content.transform.GetChild(i).gameObject.GetComponent<UIShopSlot>();
            UIslot.Rebuild(items[i].id);
        }
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());    

    }
}
