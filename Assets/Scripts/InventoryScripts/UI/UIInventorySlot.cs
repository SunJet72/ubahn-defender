using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField] public InventorySlot realSlot;
    [SerializeField] Image slotImage;
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text itemCountText;

    public void RefreshSlot()
    {
        itemNameText.text = realSlot.GetSample().title;
        int count = realSlot.Count;
        itemCountText.text = count == 1?"":realSlot.Count.ToString();
        slotImage.sprite = realSlot.GetSample().sprite;
    }
 
}
