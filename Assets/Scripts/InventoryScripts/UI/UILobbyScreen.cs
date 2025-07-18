using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyScreen : MonoBehaviour
{
    [SerializeField] UIInventoryController uiInventory;
    [SerializeField] UIShopManager uiShop;
    [SerializeField] TMP_Text playerNickname;
    [SerializeField] TMP_Text playerMoney;
    [SerializeField] Button readyButton;
    private Station currentStation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStation = WorldMapController.instance.currentStation;
        Rebuild();
        PlayerInventory.instance.MoneyChanged.AddListener(UpdateMoney);
    }

    public void Rebuild()
    {
        playerNickname.text = PlayerInventory.instance.GetNickname();
        StringBuilder str = new StringBuilder();
        playerMoney.text = str.Append("Player`s Money: ").Append(PlayerInventory.instance.GetMoney()).Append(" $").ToString();
        uiInventory.Rebuild();
        uiShop.Rebuild(currentStation);
    }

    public void UpdateMoney()
    {
        StringBuilder str = new StringBuilder();
        playerMoney.text = str.Append("Player`s Money: ").Append(PlayerInventory.instance.GetMoney()).Append(" $").ToString();
    }

}
