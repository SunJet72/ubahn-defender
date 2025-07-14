using UnityEngine;

public class GameCombatUIManager : MonoBehaviour
{
    [SerializeField] private SpellButton armorSpellButton;
    [SerializeField] private SpellButton weaponSpellButton;

    public void SetSpells(Spell spellArmor, Spell spellWeapon)
    {
        armorSpellButton.SetSpell(spellArmor);
        weaponSpellButton.SetSpell(spellWeapon);
    }
}
