using System.Collections.Generic;
using UnityEngine;

public enum EffectTargetFilter
{
    SELF,
    ENEMIES,
    VEHICLES,
    ALLIES,

}
[System.Serializable]
public struct EffectStageInfo
{
    public float timeStamp;
    public EffectExecutor spellExecution;
}

[CreateAssetMenu(fileName = "EffectData", menuName = "Scriptable Objects/EffectData")]
public class EffectData : ScriptableObject
{
    public List<Filter> filters;
    public float effectDuration; // Beginning from spawning, not from first stage. Ending not neccesarily when last timestamp
    public List<EffectStageInfo> effectStagesTimeStamp; // having 0 means effect has only animation, but no influence.
    public GameObject _effectStatefulExecutorPrefab; // Has component Stateful Executor
}
