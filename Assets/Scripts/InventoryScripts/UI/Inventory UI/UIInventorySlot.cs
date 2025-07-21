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
    [SerializeField] private Color commonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color legendaryColor;

    public void RefreshSlot()
    {
        itemNameText.text = realSlot.GetSample().title;
        int count = realSlot.Count;
        itemCountText.text = count == 1 ? "" : realSlot.Count.ToString();
        slotImage.sprite = realSlot.GetSample().sprite;
        switch (realSlot.GetSample().tier)
        {
            case 0:
                slotImage.color = commonColor;
                break;
            case 1:
                slotImage.color = rareColor;
                break;
            case 2:
                slotImage.color = legendaryColor;
                break;

            default:
                slotImage.color = commonColor;
                break;
        }
    }
 
}
