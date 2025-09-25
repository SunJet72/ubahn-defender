using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class StatefulExecutor : NetworkBehaviour
{
    private class TickTimerActioned
    {
        public TickTimer tickTimer;
        public Action<StatefulExecutor> action;

        public TickTimerActioned()
        {
            tickTimer = TickTimer.None;
            action = null;
        }
    }
    private List<TickTimerActioned> tickTimerActioneds;
    //public abstract EffectExecutorData SpellExecutorData { get; }
    public Action OnStartExecution;
    public Action OnEndExecution;
    private Action<StatefulExecutor, int> OnSpellTimerExpired;
    private UnitController caster;
    private UnitController target;
    private Vector2 end;
    private int spellStage;

    [Networked]
    private TickTimer spellTimer { get; set; }

    public void Initialize(UnitController caster, UnitController target)
    {
        this.caster = caster;
        this.target = target;
        spellStage = 0;
        tickTimerActioneds = new List<TickTimerActioned>();
        StartExecution();
    }
    public void Initialize(UnitController caster, Vector2 end)
    {
        this.caster = caster;
        this.end = end;
        spellStage = 0;
        tickTimerActioneds = new List<TickTimerActioned>();
        StartExecution();
    }

    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsServer) return;

        foreach (var ta in tickTimerActioneds)
        {
            if (!Equals(ta.tickTimer, TickTimer.None) && ta.tickTimer.Expired(Runner))
            {
                ta.action.Invoke(this);
                ResetTimerActioned(ta);
            }
        }

        if (Equals(spellTimer, TickTimer.None) || !spellTimer.Expired(Runner)) return;

        OnSpellTimerExpired?.Invoke(this, spellStage);
        ResetTimer();
    }

    private void StartExecution()
    {
        OnStartExecution?.Invoke();
    }
    private void EndExecution()
    {
        OnEndExecution?.Invoke();
        Runner.Despawn(Object);
    }

    public Action<StatefulExecutor, int> SetStageTimer(float time)
    {
        OnSpellTimerExpired = null;
        spellTimer = TickTimer.CreateFromSeconds(Runner, time);
        return OnSpellTimerExpired;
    }

    public Action<StatefulExecutor> SetNormalTimer(float time)
    {
        TickTimerActioned ta = new TickTimerActioned();
        ta.tickTimer = TickTimer.CreateFromSeconds(Runner, time);
        return ta.action;
    }

    private void ResetTimer()
    {
        spellTimer = TickTimer.None;
        spellStage++;
    }

    private void ResetTimerActioned(TickTimerActioned ta)
    {
        ta.tickTimer = TickTimer.None;
        ta.action = null;
    }

    public void Kill()
    {
        EndExecution();
    }

    public UnitController GetCaster()
    {
        return caster;
    }
    public UnitController GetTarget()
    {
        return target;
    }
    public Transform GetCastTransform()
    {
        return caster.transform;
    }
    public Vector2 GetEndPoint()
    {
        return end;
    }
}
