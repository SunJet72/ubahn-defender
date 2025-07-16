using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIDescriptionWindow : MonoBehaviour
{
    [SerializeField] TMP_Text itemTitle;
    [SerializeField] TMP_Text itemDescription;
    [SerializeField] Image itemSprite;
    [SerializeField] TMP_Text spellDescription;
    [SerializeField] Image spellSprite;
    [SerializeField] GameObject content;
    public void Rebuild(ScriptableItemBase item)
    {
        /*
        content.SetActive(true);
        itemTitle.text = item.title;
        itemSprite.sprite = item.sprite;
        StringBuilder str = new StringBuilder();
        if (item is ScriptableWeapon)
        {
            BuildWeponDescription(str, (ScriptableWeapon)item);
        }
        else if (item is ScriptableArmor)
        {
            BuildArmorDescription(str, (ScriptableArmor)item);
        }
        else if (item is ScriptableConsumable)
        {
            //Do Nothing
        }
        else
        {
            Debug.LogError("How? How did you get here, you sick bastard. There are only three types of items!");
        }
        str.Append("\n").Append(item.description);
        itemDescription.text = str.ToString();
        str.Clear();
        if (item.action == null)
        {
            spellSprite.gameObject.SetActive(false);
            spellDescription.gameObject.SetActive(false);
        }
        else
        {
            spellSprite.gameObject.SetActive(true);
            spellDescription.gameObject.SetActive(true);
            spellSprite.sprite = item.action.actionSprite;
            spellDescription.text = str.Append(item.action.description).ToString();
        }
        */
    }

    private void BuildWeponDescription(StringBuilder str, ScriptableWeapon weapon)
    {
        str.Append("Weapon Damadge: ").Append(weapon.damage).Append("\nWeapon Attackspeed: ").Append(weapon.attackSpeed).Append("Weapon Armor Penetration: ").Append(weapon.armorPenetration);
    }
    
    private void BuildArmorDescription(StringBuilder str, ScriptableArmor armor)
    {
        str.Append("Armor additional Health: ").Append(armor.additionalHealth).Append("\nArmor Resistance").Append(armor.armor);
    }

    public void HidePanel()
    {
        content.SetActive(false);
    }
}
