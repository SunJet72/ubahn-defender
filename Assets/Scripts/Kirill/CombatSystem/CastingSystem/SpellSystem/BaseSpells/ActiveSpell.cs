using System;
using Fusion;
using UnityEngine;

public abstract class ActiveSpell : Spell
{

    public void Activate(UnitController caster, Vector2 end)
    {
        ExecuteRpc(caster, end);
        //reload.SpellWasUsed();
    }

    protected void ExecuteRpc(UnitController caster, Vector2 end)
    {
        // TODO: create different methodes to get StartTransform: PlayerTransform, CreatedObject(Projectile), or create new Transform from EndPoint
        Transform start = GetStartTransform(caster, end);

        NetworkObject no = caster.Runner.Spawn((SpellData as ActiveSpellData)._spellStatefulExecutorPrefab, onBeforeSpawned: (runner, spawned) =>
        {
            spawned.transform.parent = caster.transform;
            spawned.transform.localPosition = Vector2.zero;
            spawned.transform.up = (end - (Vector2)start.position).normalized;
        });
        StatefulExecutor executor = no.GetComponent<StatefulExecutor>();
        executor.Initialize(caster, end);

        PrepareSpell(executor, 0);
    }

    protected abstract Transform GetStartTransform(UnitController caster, Vector2 end); // For now forces to spawn projectiles with no delay TODO: Reconsider this


    private void PrepareSpell(StatefulExecutor executor, int stage)
    {
        Action<StatefulExecutor, int> act;
        if (stage >= (SpellData as ActiveSpellData).spellStageInfos.Count)
        {
            if (stage == 0)
                act = executor.SetStageTimer((SpellData as ActiveSpellData).spellDuration);
            else
                act = executor.SetStageTimer((SpellData as ActiveSpellData).spellDuration - (SpellData as ActiveSpellData).spellStageInfos[stage].timeStamp);
            act += EndEffect;
        }
        else
        {
            if (stage == 0)
                act = executor.SetStageTimer((SpellData as ActiveSpellData).spellStageInfos[stage].timeStamp);
            else
                act = executor.SetStageTimer((SpellData as ActiveSpellData).spellStageInfos[stage].timeStamp - (SpellData as ActiveSpellData).spellStageInfos[stage - 1].timeStamp);
            act += ExecuteSpell;
        }

    }

    private void ExecuteSpell(StatefulExecutor executor, int spellStage)
    {
        ExecuteSpell(executor.GetCaster(), executor.GetCastTransform(), executor.GetEndPoint(), spellStage);
        PrepareSpell(executor, spellStage + 1);
    }

    private void ExecuteSpell(UnitController caster, Transform castTransform, Vector2 endPoint, int stage) // Stage begins with 0
    {
        SpellStageInfo spellStageInfo = (SpellData as ActiveSpellData).spellStageInfos[stage];
        ExecuteOnBeginExecutionEffects(spellStageInfo.spellExecution, caster);
        foreach (UnitController hit in spellStageInfo.spellExecution.Perform(caster, castTransform, endPoint))
        {
            ExecuteOnHitExecutionEffects(spellStageInfo.spellExecution, caster, hit);
        }
    }
    
    private void EndEffect(StatefulExecutor executor, int _)
    {
        executor.Kill();
        // TODO: Just look it works
    }

    private void ExecuteOnBeginExecutionEffects(SpellExecutor execution, UnitController caster)
    {
        foreach (Effect effect in execution.SpellExecutorData.OnBeginEffects)
            ApplyEffect(caster, caster, effect);
    }

    private void ExecuteOnHitExecutionEffects(SpellExecutor execution, UnitController caster, UnitController target)
    {
        foreach (Effect effect in execution.SpellExecutorData.OnHitEffects)
            ApplyEffect(caster, target, effect);
    }

}
