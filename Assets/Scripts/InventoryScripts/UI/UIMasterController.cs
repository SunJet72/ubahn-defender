using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIMasterController : MonoBehaviour
{
    public static UIMasterController instance;
    private UIConsumableSelector consumableSelector;
    private UIShopManager uiShop;
    private UIInventoryController inventoryController;
    private UIDescriptionWindow descWindow;

    private UILobbyScreen lobbyScreen;

    public DummyPlayerManager player;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        consumableSelector = GetComponentInChildren<UIConsumableSelector>();
        //inventoryController = GetComponentInChildren<UIInventoryController>();
        player = GameObject.Find("Player").GetComponent<DummyPlayerManager>();
        uiShop = GetComponentInChildren<UIShopManager>();
        lobbyScreen = GetComponentInChildren<UILobbyScreen>();
        descWindow = GetComponentInChildren<UIDescriptionWindow>();
    }
    public void RebuildConsumableSelector()
    {
        consumableSelector?.Rebuild(player);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public void RebuildAll()
    {
        consumableSelector?.Rebuild(player);
        lobbyScreen?.Rebuild();
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public void RebuildLobby()
    {
        lobbyScreen?.Rebuild();
    }

    public void RebuildShop(Station station)
    {
        uiShop?.Rebuild(station);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public void ShowDescription(ScriptableItemBase item)
    {
        descWindow?.ShowPanel(item);
    }

    public UILobbyScreen GetLobbyScreen()
    {
        return lobbyScreen;
    }
    
}
