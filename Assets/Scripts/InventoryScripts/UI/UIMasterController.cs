using UnityEngine;
using UnityEngine.Events;

public class UIMasterController : MonoBehaviour
{
    public static UIMasterController instance;
    private UIConsumableSelector consumableSelector;
    private UIInventoryController inventoryController;

    public DummyPlayerManager player;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        consumableSelector = GetComponentInChildren<UIConsumableSelector>();
        inventoryController = GetComponentInChildren<UIInventoryController>();
        player = GameObject.Find("Player").GetComponent<DummyPlayerManager>();
    }

    public void RebuildConsumableSelector()
    {
        consumableSelector.Rebuild(player);
    }

    public void RebuildAll()
    {
        consumableSelector.Rebuild(player);
        inventoryController.Rebuild();
    }
}
