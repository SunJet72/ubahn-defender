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
    public List<StatusEffect> statusEffects;
    public EffectorSpellType type;
}
