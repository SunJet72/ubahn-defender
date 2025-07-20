using System.Collections.Generic;
using System.Text;
using LightScrollSnap;
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
    [SerializeField] ScrollSnap scrollSnap;

    [SerializeField] TMP_Dropdown directionChooser;

    private List<Station> optionMpper = new List<Station>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        LeaveStation();
    }
    private bool shopEnabled;
    void Start()
    {
        Rebuild();
        PlayerInventory.instance.MoneyChanged.AddListener(UpdateMoney);
    }

    public void Rebuild()
    {
        if (WorldMapController.instance.isOnStation)
        {
            if (!shopEnabled)
            {
                shopEnabled = true;
                ArriveAtStation(WorldMapController.instance.currentStation);
            }
            uiShop?.Rebuild(WorldMapController.instance.currentStation);
        }
        else
        {
            if (shopEnabled)
            {
                shopEnabled = false;
                LeaveStation();
            }
        }
        
        playerNickname.text = PlayerInventory.instance.GetNickname();
        StringBuilder str = new StringBuilder();
        playerMoney.text = str.Append("Player`s Money: ").Append(PlayerInventory.instance.GetMoney()).Append(" $").ToString();
        uiInventory.Rebuild();
    }

    public void UpdateMoney()
    {uiShop.gameObject.SetActive(true);
        StringBuilder str = new StringBuilder();
        playerMoney.text = str.Append("Player`s Money: ").Append(PlayerInventory.instance.GetMoney()).Append(" $").ToString();
    }

    public void ArriveAtStation(Station station)
    {
        uiShop.gameObject.SetActive(true);
        readyButton.gameObject.SetActive(true);
        scrollSnap.SmoothScrollToItem(uiShop.gameObject.transform.GetSiblingIndex(), scrollSnap.smoothScrollDuration);
        directionChooser.gameObject.SetActive(true);
        foreach (UBahnLine ulin in station.uBahnLines) {
            foreach (Station st in station.Neighbours) {
                if (st.uBahnLines.Contains(ulin))
                {
                    StringBuilder str = new StringBuilder();
                    switch (ulin)
                    {
                        case UBahnLine.U1:
                            str.Append("U1");
                            break;
                        case UBahnLine.U2:
                            str.Append("U2");
                            break;
                        case UBahnLine.U3:
                            str.Append("U3");
                            break;
                        case UBahnLine.U4:
                            str.Append("U4");
                            break;
                        case UBahnLine.U5:
                            str.Append("U5");
                            break;
                        case UBahnLine.U6:
                            str.Append("U6");
                            break;
                        case UBahnLine.U7:
                            str.Append("U7");
                            break;
                        case UBahnLine.U8:
                            str.Append("U8");
                            break;
                        default:
                            str.Append("Unknown line");
                            break;
                    }
                    str.Append(" to ").Append(st.StationName);
                    TMP_Dropdown.OptionData optdata = new TMP_Dropdown.OptionData(str.ToString(), ItemManager.instance.defaultSprite, Color.white);
                    directionChooser.options.Add(optdata);
                    optionMpper.Add(st);
                    GameFlowManager.instance.nextStation = optionMpper[0];
                }
            }
        }
        directionChooser.RefreshShownValue();
    }

    public void DirectionChooserMapper(int optionIndex)
    {
        Debug.Log(optionMpper[optionIndex].StationName);
        GameFlowManager.instance.nextStation = optionMpper[optionIndex];

    }

    public void LeaveStation()
    {
        uiShop.gameObject.SetActive(false);
        readyButton.gameObject.SetActive(false);
        directionChooser.ClearOptions();
        optionMpper.Clear();
        directionChooser.gameObject.SetActive(false);
    }
}
