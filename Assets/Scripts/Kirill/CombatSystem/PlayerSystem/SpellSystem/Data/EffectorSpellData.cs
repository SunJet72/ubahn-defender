using System.Collections.Generic;
using UnityEngine;
public enum EffectorSpellType
{
    SELF,
    TARGET,
    AREA
}
[CreateAssetMenu(fileName = "EffectorSpellData", menuName = "Scriptable Objects/EffectorSpellData")]
public class EffectorSpellData : SpellData
{
    [Header("Effector Spell Info")]
    public List<StatusEffect> statusEffects;
    public EffectorSpellType type;
}
