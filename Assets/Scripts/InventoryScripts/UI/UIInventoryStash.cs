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
        parentRT = GetComponentInParent<RectTransform>();
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

        //float rowSlotCount = Mathf.Floor(parentRT.rect.width / gl.cellSize.x) + 0.1f;
        //float rowCount = Mathf.Ceil(slots.Count / rowSlotCount);
        // Debug.Log(rowCount+" "+ rowSlotCount);
        //rt.sizeDelta = new Vector2(parentRT.rect.width, gl.cellSize.y*rowCount);
        
        for(int diff = transform.childCount -1; diff>= slots.Count; --diff)
        {
            Destroy(transform.GetChild(diff).gameObject);
        }
        
        for (int diff = transform.childCount;diff<slots.Count;diff++)
        {
            GameObject obj = Instantiate(UISlotPrefab, transform);
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
        Rebuild((slots??new List<InventorySlot>()).Where(a => a != null && a.Sample is ScriptableArmor).ToList());
    }

    public void ShowWeaponOptions(List<InventorySlot> slots)
    {
        Rebuild((slots??new List<InventorySlot>()).Where(a => a != null && a.Sample is ScriptableWeapon).ToList());
    }
}
