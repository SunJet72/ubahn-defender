using UnityEngine;
using UnityEngine.UI;

public class UIConsumableSelector : MonoBehaviour
{
    [SerializeField] private GameObject UIConsumablePrefab;

    [SerializeField] private GameObject content;

    [SerializeField] private UIInventorySlot currentConsumable;

    private bool ListOpened = false;

    void Awake()
    {
        content.transform.parent.gameObject.SetActive(ListOpened);
    }

    public void RebuildActiveSlot(DummyPlayerManager player)
    {
        currentConsumable.realSlot = player.activeConsumables[player.ActiveConsumableIndex];
        currentConsumable.RefreshSlot();
    }

    public void Rebuild(DummyPlayerManager player)
    {
        RebuildActiveSlot(player);

        for (int diff = content.transform.childCount - 1; diff >= player.activeConsumables.Length; --diff)
        {
            Destroy(content.transform.GetChild(diff).gameObject);
        }

        for (int diff = content.transform.childCount; diff < player.activeConsumables.Length; diff++)
        {
            GameObject obj = Instantiate(UIConsumablePrefab, content.transform);
        }

        for (int i = 0; i < player.activeConsumables.Length; ++i)
        {
            int index = i;

            UIInventorySlot UIslot = content.transform.GetChild(i).gameObject.GetComponent<UIInventorySlot>();
            UIslot.realSlot = player.activeConsumables[i];
            UIslot.RefreshSlot();
            Button btn = content.transform.GetChild(i).gameObject.GetComponent<Button>();
            Debug.Log(btn.gameObject.GetComponent<UIInventorySlot>().realSlot.GetSample().title);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => { player.ActiveConsumableIndex = index; RebuildActiveSlot(player);});
        }
    }


    public void ToggleList()
    {
        content.transform.parent.gameObject.SetActive(!ListOpened);
        ListOpened = !ListOpened;
    }

}
