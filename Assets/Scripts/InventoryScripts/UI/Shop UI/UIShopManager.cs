using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopManager : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject TierShopPrefab;

    public void Rebuild(Station currentStation)
    {

        for (int diff = content.transform.childCount - 1; diff >= currentStation.StationTier + 1; --diff)
        {
            Destroy(content.transform.GetChild(diff).gameObject);
        }

        for (int diff = content.transform.childCount; diff < currentStation.StationTier+1; diff++)
        {
            GameObject obj = Instantiate(TierShopPrefab, content.transform);
        }

        for (int i = 0; i <currentStation.StationTier+1; ++i)
        {
            UIShopItemTier UItier = content.transform.GetChild(i).gameObject.GetComponent<UIShopItemTier>();
            UItier.Rebuild(ItemManager.instance.GetAll(i), i);
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
