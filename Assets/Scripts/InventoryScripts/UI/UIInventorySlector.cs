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
        
        // StringBuilder str = new StringBuilder();
        // str.Append("Clicked on me, here is the stack:\n");
        foreach (GameObject go in eventData.hovered)
        {
            //str.Append(go.name + "\n");
            switch (state)
            {
                case SelectorState.NoSelection:
                    UIItemSlot itemSlot;

                    if (go.TryGetComponent<UIItemSlot>(out itemSlot))
                    {
                        //Debug.Log("check " + slot);
                        if (itemSlot == controller.UIweapon)
                        {
                            state = SelectorState.SelectingWeapon;
                            selectorText.text = "Selecting Weapon:";
                            selectedSlot = itemSlot;
                            controller.Rebuild();
                        }
                        else if (itemSlot == controller.UIarmor)
                        {
                            state = SelectorState.SelectingArmor;
                            selectorText.text = "Selecting Armor:";
                            selectedSlot = itemSlot;
                            controller.Rebuild();
                        }
                        else
                        {
                            Debug.LogError("Shit, what have you done. How did you select this slot");
                        }
                        StringBuilder str = new StringBuilder();
                        // str.Append("Clicked on me, here is the stack:\n");
                        return;
                    }

                    break;
                case SelectorState.SelectingArmor:
                case SelectorState.SelectingWeapon:

                    UIInventorySlot slot;

                    if (go.TryGetComponent<UIInventorySlot>(out slot))
                    {
                        if (slot.realSlot.Sample is ScriptableWeapon && state == SelectorState.SelectingWeapon)
                        {
                            state = SelectorState.NoSelection;
                            selectedSlot = null;
                            inventory.SwapOutCurrentWeapon(slot.realSlot.Sample);
                            selectorText.text = "Inventory: ";
                            //controller.Rebuild();
                            return;

                        }
                        if (slot.realSlot.Sample is ScriptableArmor && state == SelectorState.SelectingArmor)
                        {
                            state = SelectorState.NoSelection;
                            selectedSlot = null;
                            inventory.SwapOutCurrentArmor(slot.realSlot.Sample);
                            selectorText.text = "Inventory: ";

                            //controller.Rebuild();
                            return;
                        }
                        Debug.LogError("Smthing went wrong during swap out. Maybe a wrong item. Bro just check some good vibe");
                        state = SelectorState.NoSelection;
                        selectedSlot = null;
                        selectorText.text = "Inventory: ";

                        return;
                    }
                    break;
            }
        }
        state = SelectorState.NoSelection;
        selectedSlot = null;
        controller.Rebuild();
        selectorText.text = "Inventory: ";
        //Debug.Log(str.ToString());
        
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
