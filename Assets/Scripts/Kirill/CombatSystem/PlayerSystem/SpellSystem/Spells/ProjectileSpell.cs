using Fusion;
using UnityEngine;

public class ProjectileSpell : ActiveSpell
{
    [SerializeField] private ProjectileSpellData data;
    [SerializeField] private GameObject _projectileSpellExecutor;

    public override SpellData SpellData => data;
    
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    protected override void ExecuteRpc(NetworkObject playerNO, NetworkObject nStart, Vector2 end)
    {
        PlayerCombatSystem player = playerNO.GetComponent<PlayerCombatSystem>();
        Transform start = nStart.transform;

        NetworkObject no = Runner.Spawn(_projectileSpellExecutor, onBeforeSpawned: (runner, spawned) =>
        {
            spawned.transform.parent = transform;
            spawned.transform.localPosition = Vector2.zero;
            spawned.transform.up = (end - (Vector2)start.position).normalized;
        });

        ProjectileSpellExecutor executor = no.GetComponent<ProjectileSpellExecutor>();

        if (data.targetType == TargetType.CURRENT_TARGET)
        {
            executor.Initialize(data, start, player.GetCurrentTargetSelected().transform, player);
        }
        else
        {
            executor.Initialize(data, start, end, player);
        }
    }
}
