using UnityEngine;
// DEPRECATED
public class GameCombatUIManager : MonoBehaviour
{
    [SerializeField] private SpellButton armorSpellButton;
    [SerializeField] private SpellButton weaponSpellButton;

    public void SetSpells(PlayerCombatSystem player, Spell spellArmor, Spell spellWeapon)
    {
        armorSpellButton.SetSpell(player, spellArmor);
        weaponSpellButton.SetSpell(player, spellWeapon);
    }
}
