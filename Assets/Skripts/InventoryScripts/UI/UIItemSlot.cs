using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemSlot : MonoBehaviour
{
    [SerializeField] Image slotImage;
    [SerializeField] TMP_Text itemNameText;

    public void RefreshSlot(ScriptableItemBase newItem)
    {
        itemNameText.text = newItem.title;
        slotImage.sprite = newItem.sprite;
    }

}
