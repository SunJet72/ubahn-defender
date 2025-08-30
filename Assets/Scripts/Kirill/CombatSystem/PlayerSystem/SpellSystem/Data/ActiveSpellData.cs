using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSpellData", menuName = "Scriptable Objects/ActiveSpellData")]
public class ActiveSpellData : SpellData
{
    [Header("Active Spell Info")]
    public List<Effect> OnBeginExecutionEffects;
    public List<Effect> OnHitEffects;
    public List<Effect> OnEndExecutionEffects;
    public GameObject _spellExecutorPrefab; // Has Component SpellExecutor
}
