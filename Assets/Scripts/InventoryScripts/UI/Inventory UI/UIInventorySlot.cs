using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField] public InventorySlot realSlot;
    [SerializeField] Image slotImage;
    [SerializeField] Image backgroundPanel; 
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
                backgroundPanel.color = commonColor;
                break;
            case 1:
                backgroundPanel.color = rareColor;
                break;
            case 2:
                backgroundPanel.color = legendaryColor;
                break;

            default:
                backgroundPanel.color = commonColor;
                break;
        }
    }
 
}
