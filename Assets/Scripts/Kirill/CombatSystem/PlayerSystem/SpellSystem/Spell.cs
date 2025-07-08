using UnityEngine;

[CreateAssetMenu(fileName = "NAME_GeneralSpellData", menuName = "Spells/General Spell Data")]
public class Spell : ScriptableObject
{
    public string spellName;
    public string spellDescription;
    public Sprite icon;
}
