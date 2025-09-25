using System;
using Fusion;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Scriptable Objects/Effect")]
public abstract class Effect : ScriptableObject
{
    // BEWARE OF: Effects dont have instances. So every unit that shares same effect, share same SO. => You cannot store here local dynamic data.
    public abstract EffectData EffectData { get; }
    public void ApplyEffect(UnitController caster, UnitController target)
    {
        bool isFit = true;
        foreach (Filter filter in EffectData.filters)
        {
            isFit &= filter.Check(caster, target);
        }

        if (isFit && EffectData.filters.Count > 0)
        {
            SpawnExecutor(caster, target);
        }
    }

    private void SpawnExecutor(UnitController caster, UnitController target)
    {
        NetworkObject no = caster.Runner.Spawn(EffectData._effectStatefulExecutorPrefab, onBeforeSpawned: (runner, spawned) =>
        {
            spawned.transform.parent = target.transform;
            spawned.transform.localPosition = Vector2.zero;
            //spawned.transform.up = (target.transform.position - caster.transform.position).normalized;
        });

        StatefulExecutor executor = no.GetComponent<StatefulExecutor>();
        //SubscribeOnExecutorEvents(executor);
        executor.Initialize(caster, target);
        PrepareEffect(executor, 0);
    }

    private void PrepareEffect(StatefulExecutor executor, int stage)
    {
        Action<StatefulExecutor, int> act;
        if (stage >= EffectData.effectStagesTimeStamp.Count)
        {
            if (stage == 0)
                act = executor.SetStageTimer(EffectData.effectDuration);
            else
                act = executor.SetStageTimer(EffectData.effectDuration - EffectData.effectStagesTimeStamp[stage].timeStamp);
            act += EndEffect;
        }
        else
        {
            if (stage == 0)
                act = executor.SetStageTimer(EffectData.effectStagesTimeStamp[stage].timeStamp);
            else
                act = executor.SetStageTimer(EffectData.effectStagesTimeStamp[stage].timeStamp - EffectData.effectStagesTimeStamp[stage - 1].timeStamp);
            act += PrepExecuteEffect;
        }

    }

    private void PrepExecuteEffect(StatefulExecutor executor, int spellStage)
    {
        ExecuteEffect(executor, spellStage);
        PrepareEffect(executor, spellStage + 1);
    }

    protected abstract void ExecuteEffect(StatefulExecutor executor, int stage); // Stage begins with 0.
    
    private void EndEffect(StatefulExecutor executor, int _)
    {
        executor.Kill();
        // TODO: Just look it works
    }
}
