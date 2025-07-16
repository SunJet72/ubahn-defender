using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;

public class UIStationScreen : MonoBehaviour
{
    [SerializeField] UIInventoryController uiInventory;
    [SerializeField] UIShopManager uiShop;
    [SerializeField] TMP_Text welcomeText;
    [SerializeField] TMP_Text stationStats;
    [SerializeField] TMP_Text playerStats;
    private Station currentStation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStation = WorldMapController.instance.currentStation;
        Rebuild();
    }

    public void Rebuild()
    {
        StringBuilder str = new StringBuilder();
        welcomeText.text = str.Append("Welcome to ").Append(currentStation.StationName).ToString();
        stationStats.text = str.Clear().Append("Station wealth: ").Append(currentStation.Wealth).Append(" $\n").Append("Station tier: ").Append(currentStation.StationTier).ToString();
        //playerStats.text = str.Clear().Append()
        //uiInventory.Rebuild();
        uiShop.Rebuild(currentStation);
    }

}
