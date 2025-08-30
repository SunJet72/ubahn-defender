using System;
using Fusion;
using UnityEngine;

public class ActiveSpell : Spell
{
    [SerializeField] private ActiveSpellData data;
    public Reload Reload { get => reload; }

    public override SpellData SpellData => data;

    public event Action OnSpellActivated;
    public event Action OnSpellDeactivated;
    [SerializeField] private Reload reload;
    [SerializeField] private GameObject _spellExecutorPrefab;
    public void Activate(NetworkObject playerNO, NetworkObject nStart, Vector2 end)
    {
        if (!reload.IsReady())
            return;
        OnSpellActivated?.Invoke();
        OnSpellDeactivated += Deactivate;
        ExecuteRpc(playerNO, nStart, end);
        reload.SpellWasUsed();
    }

    protected void ExecuteRpc(NetworkObject playerNO, NetworkObject nStart, Vector2 end)
    {
        UnitController player = playerNO.GetComponent<UnitController>();
        Transform start = nStart.transform;

        NetworkObject no = Runner.Spawn(_spellExecutorPrefab, onBeforeSpawned: (runner, spawned) =>
        {
            spawned.transform.parent = transform;
            spawned.transform.localPosition = Vector2.zero;
            spawned.transform.up = (end - (Vector2)start.position).normalized;
        });

        SpellExecutor executor = no.GetComponent<SpellExecutor>();
        SubscribeOnExecutorEvents(executor);
        executor.Initialize(player, start, end);
    }

    private void SubscribeOnExecutorEvents(SpellExecutor executor)
    {
        executor.OnStartExecution += ExecuteOnBeginExecutionEffects;
        executor.OnHitExecution += ExecuteOnHitExecutionEffects;
        executor.OnEndExecution += ExecuteOnEndExecutionEffects;
    }

    private void ExecuteOnBeginExecutionEffects()
    {
        foreach (Effect effect in (SpellData as ActiveSpellData).OnBeginExecutionEffects)
            TryExecuteEffect(null, null, effect); //TODO: Find owner;
    }

    private void ExecuteOnEndExecutionEffects()
    {
        foreach (Effect effect in (SpellData as ActiveSpellData).OnEndExecutionEffects)
            TryExecuteEffect(null, null, effect); //TODO: Find owner;
    }

    private void ExecuteOnHitExecutionEffects(UnitController target)
    {
        foreach (Effect effect in (SpellData as ActiveSpellData).OnBeginExecutionEffects)
            TryExecuteEffect(null, target, effect); //TODO: Find owner;
    }

    private void Deactivate()
    {
        OnSpellDeactivated?.Invoke();
    }

}
