using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UIConsumableSlots : MonoBehaviour
{

    [SerializeField] private GameObject UISlotPrefab;
    [SerializeField] private GameObject content;
    private UIInventorySlot[] uiSlots;

    public void Rebuild(InventorySlot[] slots)
    {
        uiSlots = new UIInventorySlot[slots.Length];
        //float rowSlotCount = Mathf.Floor(parentRT.rect.width / gl.cellSize.x) + 0.1f;
        //float rowCount = Mathf.Ceil(slots.Length / rowSlotCount);
        // Debug.Log(rowCount+" "+ rowSlotCount);
        //rt.sizeDelta = new Vector2(parentRT.rect.width, gl.cellSize.y*rowCount);

        for (int diff = content.transform.childCount - 1; diff >= slots.Length; --diff)
        {
            Destroy(content.transform.GetChild(diff).gameObject);
        }

        for (int diff = content.transform.childCount; diff < slots.Length; diff++)
        {
            GameObject obj = Instantiate(UISlotPrefab, content.transform);
        }

        for (int i = 0; i < slots.Length; ++i)
        {
            UIInventorySlot UIslot = content.transform.GetChild(i).gameObject.GetComponent<UIInventorySlot>();
            UIslot.realSlot = slots[i];
            UIslot.RefreshSlot();
            uiSlots[i] = UIslot;
        }
    }

    public int GetIndexOfSlot(UIInventorySlot slot)
    {
        for (int i = 0; i < uiSlots.Length; ++i)
        {
            if (uiSlots[i] == slot)
            {
                return i;
            }
        }
        return -1;
    }
    

}
