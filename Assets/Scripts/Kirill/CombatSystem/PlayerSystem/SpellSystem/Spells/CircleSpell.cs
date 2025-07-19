
using Fusion;
using UnityEngine;

public class CircleSpell : ActiveSpell
{
    [SerializeField] private CircleSpellData data;
    [SerializeField] private GameObject _circleSpellExecutor;

    public override SpellData SpellData => data;

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    protected override void ExecuteRpc(NetworkObject playerNO, NetworkObject nStart, Vector2 end) // end is not used
    {
        PlayerCombatSystem player = playerNO.GetComponent<PlayerCombatSystem>();
        Transform start = nStart.transform;
    
        NetworkObject no = Runner.Spawn(_circleSpellExecutor, onBeforeSpawned: (runner, spawned) =>
        {
            spawned.transform.parent = transform;
            spawned.transform.localPosition = Vector2.zero;
            spawned.transform.up = (end - (Vector2)start.position).normalized;
        });

        CircleSpellExecutor executor = no.GetComponent<CircleSpellExecutor>();
        executor.Initialize(data, start, player);
    }
}
