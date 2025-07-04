using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemSlot : MonoBehaviour
{
    [SerializeField] Image slotImage;
    [SerializeField] TMP_Text itemNameText;
    public SlotType type = SlotType.None;
    //private UIInventorySlector selector;



    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => GetComponentInParent<UIInventorySlector>().OnSlotClick(this));
    }

    // private void Start()
    // {
    //     selector = GetComponentInParent<UIInventorySlector>();
    //     btn.onClick.AddListener(selector.OnSlotClick(this));
    // }
    public void RefreshSlot(ScriptableItemBase newItem)
    {
        itemNameText.text = newItem.title;
        slotImage.sprite = newItem.sprite;
    }

    public enum SlotType
    {
        None,
        WeaponSlot,
        ArmorSlot
    }
}
