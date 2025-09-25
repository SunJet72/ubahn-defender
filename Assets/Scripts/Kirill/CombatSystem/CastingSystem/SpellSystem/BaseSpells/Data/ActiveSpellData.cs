using System.Collections.Generic;
using Fusion;
using UnityEngine;

public enum SpellCostType
{
    NONE,
    BLOOD_POINTS,
    DIRT_POINTS,
}
[System.Serializable]
public struct SpellStageInfo
{
    public float timeStamp;
    public SpellExecutor spellExecution;
}

[CreateAssetMenu(fileName = "ActiveSpellData", menuName = "Scriptable Objects/ActiveSpellData")]
public class ActiveSpellData : SpellData
{
    [Header("Active Spell Info")]

    public List<SpellStageInfo> spellStageInfos;
    public float spellDuration;
    public float cooldown;
    public SpellCostType spellCostType;
    public int spellCost; // if spellCostType in NONE, field is irrelevant
    public NetworkObject _spellStatefulExecutorPrefab; // Has Component StatefulExecutor
}
