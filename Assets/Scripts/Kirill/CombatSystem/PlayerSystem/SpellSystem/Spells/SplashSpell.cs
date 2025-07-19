using Fusion;
using UnityEngine;

public class SplashSpell : ActiveSpell
{
    [SerializeField] private SplashSpellData data;
    [SerializeField] private GameObject _splashSpellExecutor;

    public override SpellData SpellData => data;

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    protected override void ExecuteRpc(NetworkObject playerNO, NetworkObject nStart, Vector2 end)
    {
        PlayerCombatSystem player = playerNO.GetComponent<PlayerCombatSystem>();
        Transform start = nStart.transform;
        NetworkObject go = Runner.Spawn(_splashSpellExecutor, onBeforeSpawned: (runner, spawned) =>
        {
            spawned.transform.SetParent(transform);
            spawned.transform.localPosition = Vector2.zero;
            spawned.transform.up = (end - (Vector2)start.position).normalized;
        });

        SplashSpellExecutor executor = go.GetComponent<SplashSpellExecutor>();
        executor.Initialize(data, player, start, end);
    }
}
