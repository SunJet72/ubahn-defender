using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CastType {
    DIRECTION,
    POSITION,
    STATIC,
}

[CreateAssetMenu(fileName = "SpellData", menuName = "Scriptable Objects/SpellData")]
public class SpellData : ScriptableObject
{
    [Header("Overall Spell Info")]
    public string spellName;
    public string spellDescription;
    public Sprite icon;

    [Header("Spell Type and Cast Info")]
    public float castRadius; // 0f for spells with no cast radius. !!! IT IS NOT ALWAYS A RADIUS OF SPELL. SOME SPELLS HAVE INVINITE RADIUS, BUT ALL SPELL HAVE FINITE CAST RADIUS
    public GameObject _castedVisualPrefab; // GO with Image that will show on Spell Preparation Visual
    public CastType castType;


    [Header("Spell Effects")]
    public List<Effect> OnPassiveEffects; // All spells may have passive effects. They are casted on PlayerSpawn and mostly are permanent
}
