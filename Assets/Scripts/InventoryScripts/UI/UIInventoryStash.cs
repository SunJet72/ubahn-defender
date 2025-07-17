using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryStash : MonoBehaviour
{
    [SerializeField] private GameObject UISlotPrefab;
    [SerializeField] private GameObject content;

    public void Rebuild(List<InventorySlot> slots)
    {

        //float rowSlotCount = Mathf.Floor(parentRT.rect.width / gl.cellSize.x) + 0.1f;
        //float rowCount = Mathf.Ceil(slots.Count / rowSlotCount);
        // Debug.Log(rowCount+" "+ rowSlotCount);
        //rt.sizeDelta = new Vector2(parentRT.rect.width, gl.cellSize.y*rowCount);

        for (int diff = content.transform.childCount - 1; diff >= slots.Count; --diff)
        {
            Destroy(content.transform.GetChild(diff).gameObject);
        }

        for (int diff = content.transform.childCount; diff < slots.Count; diff++)
        {
            GameObject obj = Instantiate(UISlotPrefab, content.transform);
        }

        for (int i = 0; i < slots.Count; ++i)
        {
            UIInventorySlot UIslot = content.transform.GetChild(i).gameObject.GetComponent<UIInventorySlot>();
            UIslot.realSlot = slots[i];
            UIslot.RefreshSlot();
        }
    }

    public void ShowArmorOptions(List<InventorySlot> slots)
    {
        Rebuild((slots ?? new List<InventorySlot>()).Where(a => a != null && a.GetSample() is ScriptableArmor).ToList());
    }

    public void ShowWeaponOptions(List<InventorySlot> slots)
    {
        Rebuild((slots ?? new List<InventorySlot>()).Where(a => a != null && a.GetSample() is ScriptableWeapon).ToList());
    }
    public void ShowConsumableOptions(List<InventorySlot> slots)
    {
        Rebuild((slots??new List<InventorySlot>()).Where(a => a != null && a.GetSample() is ScriptableConsumable).ToList());
    }
}
