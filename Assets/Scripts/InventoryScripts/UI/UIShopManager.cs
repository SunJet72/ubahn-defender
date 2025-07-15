using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopManager : MonoBehaviour
{
    [SerializeField] private TMP_Text welcomeText;
    [SerializeField] private TMP_Text stationStats;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject TierShopPrefab;

    private Station currStation;
    public void Rebuild()
    {
        currStation = WorldMapController.instance.currentStation;
        // welcomeText.text = "Welcome to " + currStation.StationName;
        // StringBuilder str = new StringBuilder();
        // str.Append("Sation`s Tier: ").Append(currStation.StationTier).Append("\nSation`s Wealth: ").Append(currStation.Wealth);
        // stationStats.text = str.ToString();

        for (int diff = content.transform.childCount - 1; diff >= currStation.StationTier+1; --diff)
        {
            Destroy(content.transform.GetChild(diff).gameObject);
        }

        for (int diff = content.transform.childCount; diff < currStation.StationTier+1; diff++)
        {
            GameObject obj = Instantiate(TierShopPrefab, content.transform);
        }

        for (int i = 0; i < currStation.StationTier+1; ++i)
        {
            UIShopItemTier UItier = content.transform.GetChild(i).gameObject.GetComponent<UIShopItemTier>();
            UItier.Rebuild(ItemManager.instance.GetAll(i), i);
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
