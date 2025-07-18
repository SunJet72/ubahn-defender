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
        inventoryController?.Rebuild();
        uiShop?.Rebuild(WorldMapController.instance.currentStation);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
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
}
