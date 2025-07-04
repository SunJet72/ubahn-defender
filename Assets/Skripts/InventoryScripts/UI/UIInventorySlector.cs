using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using System.Text;
using Unity.VisualScripting;

public class UIInventorySlector : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] TMP_Text selectorText;
    [SerializeField] UIItemSlot selectedSlot;
    public SelectorState state = SelectorState.NoSelection;

    private UIInventoryStash stash;

    private GameObject player;
    private PlayerInventory inventory;

    private UIInventoryController controller;

    public void Awake()
    {
        stash = GetComponentInChildren<UIInventoryStash>();
        player = GameObject.Find("Player");
        inventory = player.GetComponent<PlayerInventory>();
        controller = GetComponent<UIInventoryController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        StringBuilder str = new StringBuilder();
        str.Append("Clicked on me, here is the stack:\n");
        foreach (GameObject go in eventData.hovered)
        {
            str.Append(go.name + "\n");
            if (state == SelectorState.NoSelection)
            {
                UIItemSlot slot;

                if (go.TryGetComponent<UIItemSlot>(out slot))
                {
                    Debug.Log("check " + slot);
                    switch (slot.type)
                    {
                        case UIItemSlot.SlotType.WeaponSlot:
                            state = SelectorState.SelectingWeapon;
                            selectedSlot = slot;
                            controller.Rebuild();
                            break;
                        case UIItemSlot.SlotType.ArmorSlot:
                            state = SelectorState.SelectingArmor;
                            selectedSlot = slot;
                            controller.Rebuild();
                            break;
                    }
                    return;
                }
            }
            /*
            else
            {
                UIInventorySlot slot;

                if (go.TryGetComponent<UIInventorySlot>(out slot))
                {
                    if (slot.realSlot.Sample is ScriptableWeapon && state == SelectorState.SelectingWeapon)
                    {
                        inventory.SwapOutCurrentWeapon(slot.realSlot.Sample);
                        state = SelectorState.NoSelection;
                        selectedSlot = null;
                        controller.Rebuild();

                    }
                    if (slot.realSlot.Sample is ScriptableArmor && state == SelectorState.SelectingArmor)
                    {
                        inventory.SwapOutCurrentArmor(slot.realSlot.Sample);
                        state = SelectorState.NoSelection;
                        selectedSlot = null;

                        controller.Rebuild();

                    }
                    Debug.LogError("Smthing went wrong during swap out");
                    return;
                }
            }*/
        }
        state = SelectorState.NoSelection;
        selectedSlot = null;

        Debug.Log(str.ToString());
        
    }

    public void OnSlotClick(Component clickedComponent)
    {

        /*
        selectedSlot = slot;
        if (slot.type == UIItemSlot.SlotType.WeaponSlot)
        {
            selectorText
        }
        Debug.Log("Slot: " + slot.gameObject.name + " selected");*/
    }

    public enum SelectorState
    {
        NoSelection,
        SelectingArmor,
        SelectingWeapon
    }

}
