using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryStash : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private PlayerInventory inventory;
    [SerializeField] private GameObject UISlotPrefab;
    private RectTransform rt;
    private RectTransform slotRT;
    [SerializeField] private RectTransform parentRT;
    private GridLayoutGroup gl;
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        slotRT = UISlotPrefab.GetComponent<RectTransform>();
        parentRT = transform.parent.gameObject.GetComponent<RectTransform>();
        gl = GetComponent<GridLayoutGroup>();
    }

    void Start()
    {
        //parentRT = GetComponentInParent<RectTransform>();
    }

    void OnValidate()
    {
        //Rebuild();
    }

    void OnEnable()
    {
        //Rebuild();       
    }
    public void Rebuild(List<InventorySlot> slots)
    {

        // float rowSlotCount = Mathf.Floor(parentRT.rect.width / gl.cellSize.x) + 0.1f;
        // float rowCount = Mathf.Ceil(slots.Count / rowSlotCount);
        // Debug.Log(rowCount+" "+ rowSlotCount);
        //rt.sizeDelta = new Vector2(parentRT.rect.width, gl.cellSize.y*rowCount);
        while (transform.childCount > slots.Count)
        {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }
        while (transform.childCount < slots.Count)
        {
            Instantiate(UISlotPrefab, transform);
        }
        for (int i = 0; i < slots.Count; ++i)
        {
            UIInventorySlot UIslot = transform.GetChild(i).gameObject.GetComponent<UIInventorySlot>();
            UIslot.realSlot = slots[i];
            UIslot.RefreshSlot();
        }
    }

    public void ShowArmorOptions(List<InventorySlot> slots)
    {
        Rebuild(slots.Where(a => a.Sample is ScriptableArmor).ToList());
    }

    public void ShowWeaponOptions(List<InventorySlot> slots)
    {
        Rebuild(slots.Where(a => a.Sample is ScriptableWeapon).ToList());
    }
}
